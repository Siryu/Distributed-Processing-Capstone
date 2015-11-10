using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    [Serializable]
    internal class Routing
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public int Ping { get; set; }
     
        public Routing()
        {}

        public Routing(string IP, int Port, int Ping)
        {
            this.IP = IP;
            this.Port = Port;
            this.Ping = Ping;
        }
    }
}
