using ActorModel.Messages;
using ActorModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Actors
{
    internal class OkToWorkActor
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }

        public OkToWorkActor(string ServerIP, int ServerPort)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
        }

        public void Start()
        {
            //new Thread(x => Ready()).Start();
            ThreadPool.QueueUserWorkItem(x => Ready());
        }

        private void Ready()
        {
            DataMessage dm = new DataMessage();
            dm.Data.Key = "True";
            dm.Place = -1;
            new NetworkResponseActor(dm, ManagerActor.ServerPort, ManagerActor.ServerIP).Start();
        }
    }
}
