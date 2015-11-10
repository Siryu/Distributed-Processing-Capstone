using ActorModel.Mapping;
using ActorModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.TaskManager
{
    internal class ReduceTaskHandler
    {
        public Dictionary<long, TaskInfoCurrentlyWorking> tasks;

        public ReduceTaskHandler()
        {
            this.tasks = new Dictionary<long, TaskInfoCurrentlyWorking>();
        }

        public int Count()
        {
            return this.tasks.Count();
        }

        public void Add(long ID, byte[] DLL)
        {
            Monitor.Enter(tasks);
            try
            {
                //long ID, byte[] WorkDll, string ReturnIP, int ReturnPort
                tasks.Add(ID, new TaskInfoCurrentlyWorking(ID, DLL, null, 0));
                //tasks[ID].AddCompletedWork(MappedData);
            }
            finally
            {
                Monitor.Exit(tasks);
            }
        }

        public void AddFinishedWork(long ID, MappedAnswer[] MappedData)
        {
            if (this.tasks.ContainsKey(ID))
            {
                Monitor.Enter(this.tasks);
                try
                {
                    tasks[ID].AddCompletedWork(MappedData);
                }

                finally
                {
                    Monitor.Exit(this.tasks);
                }
            }
        }

        public TaskInfoCurrentlyWorking GetTask()
        {
            TaskInfoCurrentlyWorking task = null;
            Monitor.Enter(tasks);
            try
            {
                if(tasks.Count() > 0)
                {
                    task = tasks.First().Value;
                }
            }
            finally
            {
                Monitor.Exit(tasks);
            }
            return task;
        }

        internal void Remove(long ID)
        {
            Monitor.Enter(tasks);
            try
            {
                this.tasks.Remove(ID);
            }
            finally
            {
                Monitor.Exit(tasks);
            }
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

        public TaskInfoCurrentlyWorking GetTaskInfo()
        {
            //Console.WriteLine("the count in the reduce is : " + this.tasks[1].task.Count());
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

        //internal void MarkAsComplete(long ID, long ReduceID)
        //{
        //    if (this.tasks.ContainsKey(ID))
        //    {
        //        Monitor.Enter(this.tasks);
        //        try
        //        {
        //            for (int i = 0; i < this.tasks[ID].task.Count(); i++)
        //            {
        //                if(this.tasks[ID].task[i].ReduceKey == ReduceID)
        //                {
        //                    this.tasks[ID].task[i].isWorked = true;
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            Monitor.Exit(this.tasks);
        //        }
        //    }
        //}

        internal MappedAnswer GetAnswer(long ID)
        {
            MappedAnswer answer = null;
            if (this.tasks.ContainsKey(ID))
            {
                Monitor.Enter(this.tasks);
                try
                {
                    var answerBody = this.tasks[ID].task;
                    answer = answerBody.First(x => x.ReduceKey == long.MaxValue);
                }
                catch(Exception)
                {
                    Console.WriteLine("error with getting the answer.");

                }
                finally
                {
                    Monitor.Exit(this.tasks);
                }
            }
            return answer;
        }

        internal bool Finished(long ID)
        {
            bool finished = this.tasks[ID].OneLeft();
            return finished;
        }

        //internal bool PieceHasBeenWorked(long ID, long ReduceID)
        //{
        //    bool worked = true;

        //    Monitor.Enter(this.tasks);
        //    try
        //    {
        //        var piecesWithIDs = this.tasks[ID].task.Where(x => x.ReduceKey == ReduceID);
        //        foreach (var item in piecesWithIDs)
        //        {
        //            if (!item.isWorked)
        //            {
                      
        //                worked = false;
        //                break;
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        Monitor.Exit(this.tasks);
        //    }
        //    return worked;
        //}
    }
}
