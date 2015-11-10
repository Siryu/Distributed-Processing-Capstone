using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorModel.Messages;
using System.Threading;

namespace ActorModel.Actors
{
    internal class RecieverActor : Actor
    {
        protected Queue<Message> queue;
        private Thread WorkThread { get; set; }
        private bool CloseClass = false;
        private DateTime LastTime { get; set; }
        private double TimeElapsed { get; set; }
        WorkerActor wa = new WorkerActor(null);


        public RecieverActor()
        {
            queue = new Queue<Message>();
            LastTime = DateTime.Now;
            TimeElapsed = 0d;
        }

        public void Recieve(Message message)
        {
            lock (queue)
            {
                queue.Enqueue(message);
            }
        }

        public override void Start()
        {
            WorkThread = new Thread(x => Work());
            WorkThread.Start();
        }

        public override void Close()
        {
            CloseClass = true;
        }

        private void Work()
        {
            while (!CloseClass)
            {
                if (queue.Count > 0 && wa.needsWork())
                {
                    LastTime = DateTime.Now;
                    TimeElapsed = 0d;
                    Message mess = queue.Dequeue();
                    wa = new WorkerActor(mess);
                    wa.Start();
                }
                

                if(DateTime.Now - LastTime > TimeSpan.FromSeconds(5) && queue.Count < 1 && wa.needsWork())
                {
                    new OkToWorkActor(ManagerActor.ServerIP, ManagerActor.ServerPort).Start();
                    LastTime = DateTime.Now;
                    Thread.Sleep(500);
                }
                Thread.Sleep(5);
            }
        }
    }
}
