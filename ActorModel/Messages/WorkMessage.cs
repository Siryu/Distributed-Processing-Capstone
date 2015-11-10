using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class WorkMessage : DataMessage
    {
        public string WorkDLL { get; set; }
        public int Version { get; set; }
        
        public WorkMessage(string workDLL) : base()
        {
            this.WorkDLL = workDLL;
        }
    }
}
