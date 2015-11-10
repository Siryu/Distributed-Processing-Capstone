using ActorModel.Distributers;
using ActorModel.Interfaces;
using ActorModel.Messages;
using ActorModel.Models;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.TaskManager
{
    internal class OriginalTaskHandler
    {
        private Dictionary<long, TaskInfoOriginal> tasks;
        private long currentID { get; set; }
        private object lockCurrentID = new object();

        public OriginalTaskHandler()
        {
            currentID = 0;
            tasks = new Dictionary<long, TaskInfoOriginal>();
        }

        public TaskInfoOriginal GetPieceToWork()
        {
            return tasks.First().Value;
        }

        public int Count()
        {
            return tasks.Count();
        }

        public bool PieceHasBeenWorked(long ID, int piece)
        {
            bool beenWorked = true;
            //Monitor.Enter(tasks);
            //try
            //{
                if (tasks.ContainsKey(ID))
                {
                    beenWorked = tasks[ID].task[piece].Completed;
                    if(!beenWorked)
                        this.MarkSomeAsDone(ID, piece);
                }
            //}
            //finally
            //{
            //    Monitor.Exit(tasks);
            //}
            return beenWorked;
        }

        public long addNewTask(SetupWorkMessage message)
        {
            long ID = 0;
            ReduceResult[] answer;
      
            ID = GetNewID();
            
            if (message.distributeOption == DistributerOptions.NONE)
            {
                Assembly assembly = Assembly.Load(message.TheDLL);
                var type = typeof(IDistributer);
                var types = assembly.GetTypes().Where(x => type.IsAssignableFrom(x)).ToArray(); 
                IDistributer instanceOfMyType = (IDistributer)Activator.CreateInstance(types[0]);
                answer = instanceOfMyType.Distribute(message.Data);
            }
            else
            {
                IDistributer distributer = DistributerFactory.CreateDistributer<string>(message.distributeOption);
                answer = distributer.Distribute(message.Data);
            }
            TaskIdentifier[] finished = new TaskIdentifier[answer.Length];
            int counter = 0;
            foreach (var item in answer)
            {
                finished[counter++] = new TaskIdentifier { Completed = false, DataObject = item };
            }
           
            Monitor.Enter(tasks);
            try
            {
                this.tasks.Add(ID, new TaskInfoOriginal(ID, finished, message.TheDLL, message.ReturnIP, message.Port));
            }
            finally
            {
                Monitor.Exit(tasks);
            }

            return ID;
        }

        private void MarkSomeAsDone(long ID, int piece)
        {
            this.tasks[ID].CompletePiece(piece);
        }

        public byte[] Remove(long ID)
        {
            byte[] DLL;
            Monitor.Enter(tasks);
            try
            {
                DLL = this.tasks[ID].WorkDLL;
                this.tasks.Remove(ID);
            }
            finally
            {
                Monitor.Exit(tasks);
            }
            return DLL;
        }

        private long GetNewID()
        {
            long ID;
            Monitor.Enter(this.lockCurrentID);
            try
            {
                ID = ++currentID;
            }
            finally
            {
                Monitor.Exit(this.lockCurrentID);
            }
            return ID;
        }

        private bool isComplete(long ID)
        {
            bool isComplete = false;
            if (this.tasks.ContainsKey(ID))
            {
                Monitor.Enter(tasks);
                try
                {
                    if (tasks[ID].GetFinishedPercent() == 1d)
                    {
                        isComplete = true;
                    }
                }
                finally
                {
                    Monitor.Exit(tasks);
                }
            }
            return isComplete;
        }

        public void CheckForDone(long ID)
        {
            if(isComplete(ID))
            {
                this.tasks.Remove(ID);
            }
        }
    }
}
