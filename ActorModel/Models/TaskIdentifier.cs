using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    [Serializable]
    internal class TaskIdentifier
    {
        public bool Completed { get; set; }
        public string DistributedTo { get; set; }
        public object DataObject { get; set; }
        private DateTime LastSent { get; set; }
        private const long timer = 120000;

        public TaskIdentifier()
        {
            LastSent = DateTime.MinValue;
        }

        public void SentItem()
        {
            this.LastSent = DateTime.Now;
        }

        public bool IsReady()
        {
            return (DateTime.Now.Subtract(LastSent)).TotalMilliseconds > timer;
        }
    }
}
