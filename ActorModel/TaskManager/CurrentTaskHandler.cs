using ActorModel.Mapping;
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
    internal class CurrentTaskHandler
    {
        public Dictionary<long, TaskInfoCurrentlyWorking> tasks;

        public CurrentTaskHandler()
        {
            this.tasks = new Dictionary<long, TaskInfoCurrentlyWorking>();
        }

        public void AddTask(long ID, byte[] TheDLL)
        {
            this.tasks.Add(ID, new TaskInfoCurrentlyWorking(ID, TheDLL, null, 0));    
        }

        public void MarkAsComplete(long ID, long ReduceID)
        {
            Monitor.Enter(this.tasks);
            try
            {
                for (int i = 0; i < this.tasks[ID].task.Count(); i++)
                {
                    if(this.tasks[ID].task[i].ReduceKey == ReduceID)
                    {
                        this.tasks[ID].task[i].isWorked = true;
                    }
                }
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
        }
        
        public void AddCompletedWork(long ID, MappedAnswer[] mapping)
        {
            Monitor.Enter(tasks);
            try
            {
                if (tasks.ContainsKey(ID))
                {
                    tasks[ID].AddCompletedWork(mapping);
                }
            }
            finally
            {
                Monitor.Exit(tasks);
            }
        }

        public int Count()
        {
            return tasks.Count();
        }

        public TaskInfoCurrentlyWorking GetTaskInfo()
        {
            TaskInfoCurrentlyWorking ticw = null;
            Monitor.Enter(this.tasks);
            try
            {
                ticw = tasks.Values.First();
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
            return ticw;
        }

        internal bool Finished(long ID)
        {
            bool isFinished = true;
            Monitor.Enter(this.tasks);
            try
            {
                if(this.tasks.ContainsKey(ID))
                    isFinished = this.tasks[ID].Completed();
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
            return isFinished;
        }

        internal bool PieceHasBeenWorked(long ID, long ReduceID)
        {
            bool worked = true;

            Monitor.Enter(this.tasks);
            try
            {
                var piecesWithIDs = this.tasks[ID].task.Where(x => x.ReduceKey == ReduceID);
                foreach (var item in piecesWithIDs)
                {
                    if (!item.isWorked)
                    {
                        worked = false;
                        break;
                    }
                }
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
            return worked;
        }

        internal void Remove(long ID, long ReduceID)
        {
            Monitor.Enter(this.tasks);
            try
            {
                this.tasks[ID].Remove(ReduceID);
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
        }

        internal void Remove(long ID)
        {
            Monitor.Enter(this.tasks);
            try
            {
                this.tasks.Remove(ID);
            }
            finally
            {
                Monitor.Exit(this.tasks);
            }
        }
    }
}
