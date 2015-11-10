using ActorModel;
using ActorModel.Actors;
using ActorModel.Messages;
using ActorModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Server
{
    internal class Processor : RecieverActor
    {
        public int ReturnAmount { get; set; } // how many IP's to return when requested.
        public List<Routing> Locations { get; set; }
        private Thread RecieveThread;

        private bool CloseClass = false;
 
        public Processor() : base()
        {
            ReturnAmount = 30;
            Locations = new List<Routing>();
        }

        public override void Start()
        {
            RecieveThread = new Thread(x => CheckRecieve());
            RecieveThread.Start();
            //ThreadPool.QueueUserWorkItem(x => CheckRecieve());
        }

        public override void Close()
        {
            CloseClass = true;
        }

        public void CheckRecieve()
        {
            while (!CloseClass)
            {
                DataMessage dm = null;
                Message message = null;
                 
                lock (queue)
                {
                    if (queue.Count > 0)
                    {      
                        message = queue.Dequeue();
                    }
                }

                if(message != null)
                {
                    if (message is DataMessage)
                    {
                        dm = (DataMessage)message;
                    }

                    switch (dm.Data.Key.ToString())
                    {
                        case "True":
                            {
                                //Ping ping = new Ping();
                                //PingReply pr = ping.Send(dm.ReturnIP, 500);

                                lock (Locations)
                                {
                                    if (!Locations.Exists(x => x.IP == dm.ReturnIP)) 
                                        Locations.Add(new Routing(dm.ReturnIP, dm.Port, 0));
                                }
                                Console.WriteLine("Added Location " + dm.ReturnIP);
                                break;
                            }
                        case "False":
                            {
                                lock (Locations)
                                {
                                    Locations.Remove(Locations.Find(x => x.IP == dm.ReturnIP));
                                }
                                Console.WriteLine("Removed Location " + dm.ReturnIP);
                                break;
                            }
                        default:
                            {
                                RouteMessage returnMessage = new RouteMessage();
                                returnMessage.ReturnIP = dm.ReturnIP;
                                returnMessage.Port = dm.Port;
                                returnMessage.Routes = GetSomeLocations();
                                if (returnMessage.Routes.Count() != 0)
                                {
                                    new NetworkResponseActor(returnMessage).Start();
                                }
                                break;
                            }
                    }
                }
            }
        }

        public Routing[] GetSomeLocations()
        {
            List<Routing> returnSome;
            Routing[] routes;
            lock(Locations)
            {
                returnSome = Locations.Take(ReturnAmount).ToList();
                Locations.RemoveAll(x => returnSome.Contains(x));
            }
 
            int position = 0;
            int itemCount = returnSome.Count();
            if (itemCount > 0)
            {
                routes = new Routing[ReturnAmount];
                for (int i = 0; i < ReturnAmount; i++)
                {
                    if (position >= itemCount)
                    {
                        position = 0;
                    }
                    routes[i] = returnSome[position++];
                }
            }
            else
            {
                routes = new Routing[0];
            }
            return routes;
        }
    }
}
