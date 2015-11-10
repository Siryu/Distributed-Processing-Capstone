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
    internal class Worker
    {
        private Processor Processor { get; set; }
        private TaskHandler TaskHandler { get; set; }
        private Thread WorkThread { get; set; }
        private bool CloseClass = false;

        public Worker(Processor Processor)
        {
            this.Processor = Processor;
            this.TaskHandler = new TaskHandler();
        }

        public void Start()
        {
            WorkThread = new Thread(x => Work());
            WorkThread.Start();
        }

        private void Work()
        {
            while(!CloseClass)
            {
                Routing Route = GetRoute();
       
                if(Route != null && TaskHandler.SendMap(Route))
                {
                    this.Processor.Routes.Remove(Route);
                    Route = GetRoute();
                }
                if (Route != null && TaskHandler.SendMapReduce(Route))
                {
                    this.Processor.Routes.Remove(Route);
                    Route = GetRoute();
                }
                if (Route != null && TaskHandler.SendReduce(Route))
                {
                    this.Processor.Routes.Remove(Route);
                }

                Thread.Sleep(20);
            }
        }

        private Routing GetRoute()
        {
            Routing route = null;

            if (this.Processor.Routes.Count() > 0)
                route = this.Processor.Routes[0];

            return route;
        }

        public long AddNewTask(SetupWorkMessage message)
        {
            return this.TaskHandler.AddNewTask(message);
        }

        public void AddSomeFinishedWork(long ID, int Piece, MappedAnswer[] results)
        {
            this.TaskHandler.AddSomeFinishedWork(ID, Piece, results);
        }

        public void AddFinishedWork(long ID, long ReduceID, MappedAnswer[] work)
        {
            this.TaskHandler.AddFinishedWork(ID, ReduceID, work);
        }

        public ReduceResult GetFinishedWork(long ID)
        {
            return this.TaskHandler.GetFinishedWork(ID);
        }

        public void Close()
        {
            this.CloseClass = true;
        }
    }
}
