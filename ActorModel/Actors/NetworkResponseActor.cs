using ActorModel.Messages;
using ActorModel.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Actors
{
    internal class NetworkResponseActor : Actor
    {
        private Message Message { get; set; }
        private int DPort { get; set; }
        IFormatter formatter { get; set; }

        internal NetworkResponseActor(Message message)
        {
            this.Message = message;
            this.DPort = message.Port;
            this.formatter = new BinaryFormatter();
        }

        private NetworkResponseActor(Message message, int destinationPort) : this(message)
        {
            this.DPort = destinationPort;    
        }

        public NetworkResponseActor(Message message, int destinationPort, string destinationIP) : this(message, destinationPort)
        {
            message.ReturnIP = destinationIP;
        }

        public override void Start()
        {
            ThreadPool.QueueUserWorkItem(x => Send());
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        private void Send()
        {
            TcpClient tcpClient = null;
            //SslStream stream = null;
            NetworkStream stream = null;
            try
            {
                // need to set a timeout for how long to try to connect for.
                tcpClient = new TcpClient(Message.ReturnIP.ToString(), DPort);
                //stream = ClientSecureSocket.ManageServerRequest(tcpClient, Message.ReturnIP);
                stream = tcpClient.GetStream();
                //stream = SSLTest.ManageClient(tcpClient);
                if (stream != null)
                {
                    formatter.Serialize(stream, Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (tcpClient != null)
                    tcpClient.Close();
                Message = null;
            }
        }
    }
}
