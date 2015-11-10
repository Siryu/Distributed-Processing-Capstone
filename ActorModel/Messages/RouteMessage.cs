using ActorModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class RouteMessage : Message
    {
        public Routing[] Routes { get; set; }
    }
}
