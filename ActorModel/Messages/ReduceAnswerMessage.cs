using ActorModel.Mapping;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class ReduceAnswerMessage : Message
    {
        public MappedAnswer[] Result { get; set; }
        public long ReduceID { get; set; }
        public long TaskNumber { get; set; }
    }
}
