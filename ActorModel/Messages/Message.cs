using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel
{
    [Serializable]
    internal abstract class Message
    {
        public int TryCount { get; set; }
        public string ReturnIP { get; set; }
        public int Port { get; set; }
    }
}
