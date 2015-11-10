using ActorModel.Actors;
using ActorModel.Mapping;
using ActorModel.Messages;
using ActorModel.Models;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.TaskManager
{
    internal class Processor : RecieverActor
    {
        private Worker Worker { get; set; }
        private Thread WorkThread { get; set; }
        private Thread RequestThread { get; set; }
        private bool CloseClass = false;
        public List<Routing> Routes { get; set; }
        private const int RoutesToHold = 3;

        public Processor()
        {
            Worker = new Worker(this);
            this.Routes = new List<Routing>();
        }

        public override void Start()
        {
            WorkThread = new Thread(x => DoWork());
            WorkThread.Start();
            RequestThread = new Thread(x => RequestRoutes());
            RequestThread.Start();
            Worker.Start();
        }

        private void DoWork()
        {
            try
            {
                while (!CloseClass)
                {
                    Message message = null;

                    if (queue.Count > 0)
                    {
                        message = queue.Dequeue();
                    }

                    if (message != null)
                    {
                        Console.WriteLine(message.ReturnIP + " recieved message " + message.GetType());
                        if (message is SetupWorkMessage)
                        {
                            Console.WriteLine("Recieved Data Message from client");
                            long ID = Worker.AddNewTask((SetupWorkMessage)message);
                            DataMessage dm = new DataMessage();
                            dm.ReturnIP = message.ReturnIP;
                            dm.Port = message.Port;
                            dm.Data.values = ID;
                            new NetworkResponseActor(dm).Start();
                        }
                        else if (message is RouteMessage)
                        {
                            //Console.WriteLine("Recieved Data from server for routes.");
                            this.AddRoutesToList((RouteMessage)message);
                        }
                        else if(message is MapMessage)
                        {
                            MapMessage mapMessage = (MapMessage)message;
                            Console.WriteLine("Some data worked, recieved MapMessage from " + mapMessage.ReturnIP);
                            Worker.AddSomeFinishedWork(mapMessage.TaskNumber, mapMessage.Piece, (MappedAnswer[])mapMessage.Data);
                        }
                        else if(message is ReduceAnswerMessage)
                        {
                            ReduceAnswerMessage raMessage = (ReduceAnswerMessage)message;
                            Console.WriteLine("Recieved finished Reduce, recieved ReduceAnswerMessage from " + raMessage.ReturnIP);
                            Worker.AddFinishedWork(raMessage.TaskNumber, raMessage.ReduceID, raMessage.Result);
                        }
                        else if(message is DataMessage)
                        {
                            //Console.WriteLine("Sending back finsihed results");
                            DataMessage dm = (DataMessage)message;
                            if(dm.Data.Key.ToString() == "Get")
                            {
                                ReduceResult result = Worker.GetFinishedWork((long)dm.Data.values);
                                dm.Data = result;
                                new NetworkResponseActor(dm).Start();
                            }
                        }

                    }
                }
            }
            finally
            {

            }
        }

        private void AddRoutesToList(RouteMessage message)
        {
            if (this.Routes.Count < (RoutesToHold * 2))
            {
                foreach (var item in message.Routes)
                {
                    this.Routes.Add(item);
                }
            }
        }

        private void RequestRoutes()
        {      
            while (!CloseClass)
            {
                if (Routes.Count < RoutesToHold)
                {
                    DataMessage dm = new DataMessage();
                    dm.Data.Key = "Get";
                    dm.Place = -1;
                    new NetworkResponseActor(dm, ManagerActor.ServerPort, ManagerActor.ServerIP).Start();
                    Thread.Sleep(400);
                }
                Thread.Sleep(30);
            }
        }
        
        public override void Close()
        {
            CloseClass = true;
            Worker.Close();
        }
    }
}
