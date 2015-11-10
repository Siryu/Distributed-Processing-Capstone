using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class TaskInfoCurrentlyWorking : TaskInfo
    {
        public List<MappedAnswer> task;
        private int position = 0;     //the bain of my existance!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public TaskInfoCurrentlyWorking()
        {
            this.task = new List<MappedAnswer>();
        }

        public TaskInfoCurrentlyWorking(long ID) : this()
        {
            this.ID = ID;
        }

        public TaskInfoCurrentlyWorking(long ID, byte[] WorkDll, string ReturnIP, int ReturnPort) : base(ID, WorkDll, ReturnIP, ReturnPort)
        {
            this.task = new List<MappedAnswer>();
        }
        
        public void AddCompletedWork(MappedAnswer[] mappedAnswer)
        {
            Monitor.Enter(this.task);
            try
            {
                foreach (var item in mappedAnswer)
                {
                    this.task.Add(item);
                }
            }
            finally
            {
                Monitor.Exit(this.task);
            }
        }

        public MappedAnswer[] GetAnswers()
        {
            return this.task.ToArray();
        }

        private int GetNextPosition()
        {
            int localPosition = 0;

            localPosition = ++this.position < task.Count() ? this.position : 0;
            this.position = localPosition;

            //int listSize = task.Count();

            //localPosition = position + 1 >= listSize ? 0 : position + 1;
            //position = localPosition;

            if (!task[localPosition].ReadyToSend())
            {
                localPosition = -1;
            }
            if (this.Completed())
            {
                localPosition = -1;
            }
            return localPosition;
        }

        public bool Completed()
        {
            bool completed = false;
            if(this.task.Count() <= 0)
            {
                completed = true;
            }
            return completed;
        }

        public bool OneLeft()
        {
            bool oneLeft = false;
            if (this.task.Count() == 1)
            {
                oneLeft = this.task.First().ReduceKey == long.MaxValue;
            }
            return oneLeft;
        }

        internal MappedAnswer MatchReduceKey(long reduceKey, MappedAnswer work)
        {
            MappedAnswer match = null;
            Monitor.Enter(this.task);
            try
            {
                for (int i = 0; i < this.task.Count() && match == null; i++)
                {
                    if(task[i].ReduceKey == reduceKey && task[i] != work && !task[i].isWorked)
                    {
                        match = task[i];
                    }
                }
            }
            finally
            {
                Monitor.Exit(this.task);
            }
            return match;
        }

        public MappedAnswer GetPieceToWork()
        {
            MappedAnswer ma = null;
            int position = 0;
           
            Monitor.Enter(this.task);
            try
            {
                if (this.task.Count() > 0)
                {
                    position = GetNextPosition();
                    if (position != -1)
                    {
                        ma = task[position];
                    }
                }
            }
            finally
            {
                Monitor.Exit(this.task);
            }
            
            return ma;
        }

        internal void Remove(long ReduceID)
        {
            Monitor.Enter(this.task);
            try
            {
                this.task.RemoveAll(x => x.ReduceKey == ReduceID);
            }
            finally
            {
                Monitor.Exit(this.task);
            }
        }
    }
}
