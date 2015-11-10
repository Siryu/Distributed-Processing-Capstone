using ActorModel;
using ActorModel.Actors;
using ActorModel.Messages;
using ActorModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Client
{
    internal class Processor : RecieverActor
    {
        public List<Routing> Routes { get; set; }
        private int RoutesToHold { get; set; }
        private Worker worker { get; set; }
        private Thread WorkThread { get; set; }
        private Thread RequestThread { get; set; }
        private bool CloseClass = false;

        public Processor() : base()
        {
            this.RoutesToHold = 2;
            this.Routes = new List<Routing>();
        }

        public override void Start()
        {
            WorkThread = new Thread(x => DoWork());
            WorkThread.Start();
            RequestThread = new Thread(x => RequestRoutes());
            RequestThread.Start();
            //ThreadPool.QueueUserWorkItem(x => DoWork());
            //ThreadPool.QueueUserWorkItem(x => RequestRoutes());
        }

        public override void Close()
        {
            CloseClass = true;
        }

        private void DoWork()
        {
            try
            {
                while (!CloseClass)
                {
                    Message message = null;

                    Monitor.Enter(queue);
                    try
                    {
                        if (queue.Count > 0)
                        {
                            message = queue.Dequeue();
                        }
                    }
                    finally
                    {
                        Monitor.Exit(queue);
                    }
                    

                    if (message != null)
                    {
                        if (message is DataMessage)
                        {
                            Console.WriteLine("Recieved Data Message from node");
                            AddDataToFinished((DataMessage)message);
                        }
                        else if (message is RouteMessage)
                        {
                            AddRoutesToList(message);
                        }
                    }
                }
            }
            finally
            {

            }
        }

        private void AddRoutesToList(Message message)
        {
            RouteMessage rm = (RouteMessage)message;
            lock(Routes)
            {
                foreach (Routing route in rm.Routes)
                {
                    Routes.Add(route);
                }
            }
        }

        private void AddDataToFinished(DataMessage dm)
        {
            //Console.WriteLine(dm.Data.ToString());
            worker.AddCompletedWork(dm.Data, dm.Place);
        }

        public void addWorker(Worker worker)
        {
            this.worker = worker;
        }

        private void RequestRoutes()
        {
            try
            {
                while (!CloseClass)
                {
                    if (Routes.Count < RoutesToHold)
                    {
                        DataMessage dm = new DataMessage();
                        dm.Data.Key = "Get";
                        dm.Place = -1;
                        new NetworkResponseActor(dm, ManagerActor.ServerPort, ManagerActor.ServerIP).Start();
                    }
                    Thread.Sleep(1000);
                }
            }
            finally
            {

            }
        }
    }
}
