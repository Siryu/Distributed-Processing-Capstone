using ActorModel.Distributers;
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
    internal class SetupWorkMessage : Message
    {
        public byte[] TheDLL { get; set; }
        public DistributerOptions distributeOption { get; set; }
        public MappingOptions mapOption{ get; set; }
        public ReduceOptions reduceOption { get; set; }
        public object Data { get; set; }
    }
}
