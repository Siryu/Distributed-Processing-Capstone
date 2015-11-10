using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class ReduceMessage : Message
    {
        public long TaskID { get; set; }
        public long ReduceID { get; set; }
        public byte[] ReduceDLL { get; set; }
        public MappedAnswer[] Data { get; set; }
        public long TaskNumber { get; set; }
    }
}
