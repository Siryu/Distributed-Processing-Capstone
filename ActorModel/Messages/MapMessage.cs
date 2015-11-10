using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class MapMessage : Message
    {
        public MappingOptions mapOption { get; set; }
        public object Data { get; set; }
        public byte[] MapDLL { get; set; }
        public long TaskNumber { get; set; }
        public int Piece { get; set; }
    }
}
