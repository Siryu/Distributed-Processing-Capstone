using ActorModel.Actors;
using ActorModel.Messages;
using ActorModel.Models;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Client
{
    internal class Worker
    {
        private Processor Reciever { get; set; }
        private Thread WorkSender;
        private ReduceResult[] Work { get; set; }
        //private object[] CompletedWork { get; set; }
        private int currentPosition = 0;
        private bool Done = false;
        private bool CloseClass = false;
        private int Version;
        int endPostion = 0;

        private int piecesDone = 0;


        public Worker(RecieverActor Reciever, int Version)
        {
            if (Reciever is Processor)
            {
                this.Reciever = (Processor)Reciever;
            }
            this.Version = Version;
        }

        public void AddWork(ReduceResult[] Work)
        {
            this.Work = Work;
            //this.CompletedWork = new object[Work.Length];
            endPostion = this.Work.Length;
        }

        public void AddCompletedWork(ReduceResult obj, int position)
        {
            lock (Work)
            {
                if(ValidWork(position))
                {
                    {
                        piecesDone += 1;
                    }
                    Work[position] = obj;
                }
            }

            if(GetPercentDone() == 1)
            {
                this.Done = true;
            }
        }

        private bool ValidWork(int position)
        {
            return position >= 0 && position < Work.Length && Work[position].Key == null;
        }

        public float GetPercentDone()
        {
            return (float)piecesDone / (float)endPostion;
        }

        public ReduceResult[] GetCompletedWork()
        {
            while(!Done)
            {
                Thread.Yield();
            }
            return this.Work;
        }

        public void Start()
        {
            WorkSender = new Thread(x => SendWork());
            WorkSender.Start();
            //ThreadPool.QueueUserWorkItem(x => SendWork());
        }

        public void Close()
        {
            CloseClass = true;
            //WorkSender.Abort();
        }

        public void SendWork()
        {
            try
            {
                while (!CloseClass)
                {
                    Routing Route = GetRoute();

                    if (Route != null)
                    {
                        bool foundWork = false;
                        int workPosition = 0;
                        while (!foundWork && !CloseClass && Thread.CurrentThread.ThreadState != ThreadState.AbortRequested)
                        {
                            workPosition = GetNextPosition();

                            if (this.Work[workPosition].Key == null)
                            {
                                foundWork = true;
                            }
                        }

                        Console.WriteLine("Sending Work");
                        WorkMessage wm = new WorkMessage(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".dll");
                        wm.Data = this.Work[currentPosition];
                        wm.Version = Version;
                        wm.Place = currentPosition;
                        new NetworkResponseActor(wm, Route.Port, Route.IP).Start();
                    }
                }
            }
            finally
            {

            }
        }

        private int GetNextPosition()
        {
            currentPosition = currentPosition + 1 < endPostion ? ++currentPosition : 0;
            return currentPosition;
        }

        private Routing GetRoute()
        {
            Routing Route = null;

            Monitor.Enter(Reciever.Routes);
            try
            {
                if (Reciever.Routes.Count > 0)
                {
                    Route = Reciever.Routes[new Random().Next(Reciever.Routes.Count)];
                    Reciever.Routes.Remove(Route);
                }
            }
            finally
            {
                Monitor.Exit(Reciever.Routes);
            }
            return Route;
        }
    }
}
