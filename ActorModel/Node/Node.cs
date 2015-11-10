using ActorModel.Actors;
using ActorModel.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Node
{
    /// <summary>
    /// Worker Node Class used to create a Node.
    /// </summary>
    public class Node
    {
        private RecieverActor Reciever;
        private NetworkRecieverActor NetworkReciever;

        /// <summary>
        /// Basic Constructor, needs all 3 parameters.
        /// </summary>
        /// <param name="ServerIP"></param>
        /// <param name="ServerPort"></param>
        /// <param name="LocalPort"></param>
        public Node(string ServerIP, int ServerPort, int LocalPort)
        {
            ManagerActor.ServerIP = ServerIP;
            ManagerActor.ServerPort = ServerPort;
            ManagerActor.LocalPort = LocalPort;

            Reciever = new RecieverActor();
            NetworkReciever = new NetworkRecieverActor(Reciever, false, ManagerActor.LocalPort);
        }

        /// <summary>
        /// Starts the Node by opening network connections and starting the listener.
        /// </summary>
        public void Start()
        {
            Reciever.Start();
            NetworkReciever.Start();
            //ConnectionTester.TestUntilConnectionMade(ManagerActor.ServerIP, ManagerActor.ServerPort);
            new OkToWorkActor(ManagerActor.ServerIP, ManagerActor.ServerPort).Start();            
        }

        /// <summary>
        /// Closes all threads and connections on Node.
        /// </summary>
        public void Close()
        {
            NetworkReciever.Close();
            Reciever.Close();
        }
    }
}
