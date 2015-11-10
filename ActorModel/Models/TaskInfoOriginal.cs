using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class TaskInfoOriginal : TaskInfo
    {
        public TaskIdentifier[] task;
        public int position = 0;
        private int taskCount = 0;

        public TaskInfoOriginal(long ID, TaskIdentifier[] task, byte[] WorkDll, string ReturnIP, int ReturnPort) : base(ID, WorkDll, ReturnIP, ReturnPort)
        {
            this.task = task;
            this.taskCount = task.Count();
        }

        public void CompletePiece(int piece)
        {
            Monitor.Enter(this.task);
            try
            {
                if (!this.task[piece].Completed)
                {
                    this.task[piece].Completed = true;
                    this.taskFinished++;
                }
            }
            finally
            {
                Monitor.Exit(this.task);
            }
        }

        public double GetFinishedPercent()
        {
            double percent;
            Monitor.Enter(this.task);
            try
            {
                percent = ((double)taskFinished / (double)taskCount);
            }
            finally
            {
                Monitor.Exit(this.task);
            }
            return percent;
        }

        public TaskIdentifier GetPieceToWork(string IP)
        {
            TaskIdentifier ti = null;
            Monitor.Enter(this.task);
            try
            {
                int position = GetNextPosition();
                TaskIdentifier answer = null;
                if (position != -1)
                {
                    answer = task[position];
                }
                if (answer != null && !answer.Completed && answer.IsReady())
                {
                    //answer.SentItem();
                    answer.DistributedTo = IP;
                    ti = answer;
                }
            }
            finally
            {
                Monitor.Exit(this.task);
            }
            return ti;
        }

        private int GetNextPosition()
        {
            int position = 0;
            position = ++this.position < task.Length ? this.position : 0;
            this.position = position;
            if(!task[position].IsReady())
            {
                position = -1;
            }
            return position;
        }
    }
}
