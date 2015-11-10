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
    internal class FinishedTaskHandler
    {
        public Dictionary<long, TaskInfoFinished> finishedWork;

        public FinishedTaskHandler()
        {
            this.finishedWork = new Dictionary<long, TaskInfoFinished>();
        }

        public void AddFinishedWork(long ID, ReduceResult work)
        {
            if(finishedWork.ContainsKey(ID))
            {
                finishedWork.Remove(ID);
            }
            this.finishedWork.Add(ID, new TaskInfoFinished() { ID = ID, task = work });
        }

        public ReduceResult GetFinishedWork(long ID)
        {
            ReduceResult finished = null;
            Monitor.Enter(finishedWork);
            try
            {
                if (this.finishedWork.ContainsKey(ID))
                {
                    finished = this.finishedWork[ID].task;
                    this.finishedWork.Remove(ID);
                }
            }
            finally
            {
                Monitor.Exit(finishedWork);
            }
            return finished;
        }
    }
}
