using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Actors
{
    internal static class ManagerActor
    {
        public static int LocalPort { get; set; }
        public static int ServerPort { get; set; }
        public static string ServerIP { get; set; }
    }
}
