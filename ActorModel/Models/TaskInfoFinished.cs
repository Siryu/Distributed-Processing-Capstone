using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class TaskInfoFinished : TaskInfo
    {
        public ReduceResult task;

        public TaskInfoFinished()
        {

        }

        public TaskInfoFinished(long ID, ReduceResult Result, byte[] WorkDll, string ReturnIP, int ReturnPort) : base(ID, WorkDll, ReturnIP, ReturnPort)
        {

        }
    }
}
