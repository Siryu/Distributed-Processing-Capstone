using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class TaskInfoReduce : TaskInfo
    {
        public List<MappedAnswer> task;

        public TaskInfoReduce()
        {
            this.task = new List<MappedAnswer>();
        }

        public TaskInfoReduce(long ID, MappedAnswer[] task, byte[] WorkDll, string ReturnIP, int ReturnPort) : base(ID, WorkDll, ReturnIP, ReturnPort)
        {
            this.task = new List<MappedAnswer>();
            foreach (var item in task)
            {
                this.task.Add(item);
            }
        }
    }
}
