using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class TaskInfoSetup : TaskInfo
    {
        public TaskIdentifier[] task;
        
        public TaskInfoSetup(long ID, TaskIdentifier[] task, byte[] WorkDll, string ReturnIP, int ReturnPort) :
            base(ID, WorkDll, ReturnIP, ReturnPort)
        {
            this.task = task;
        }
    }
}
