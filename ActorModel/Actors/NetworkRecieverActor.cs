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
    internal class NetworkRecieverActor : Actor
    {
        public int Port { get; set; }
        public bool IsServer { get; set; }
        private int NetworkTried { get; set; }
        private RecieverActor Reciever;
        private TcpListener tcpListener;
        private IFormatter formatter;
        private Thread RecieveThread;
        private bool CloseClass = false;

        private NetworkRecieverActor(Actor Reciever, bool IsServer)
        {
            this.formatter = new BinaryFormatter();
            this.IsServer = IsServer;
            if(Reciever is RecieverActor)
                this.Reciever = (RecieverActor)Reciever;
        }

        public NetworkRecieverActor(Actor Reciever, bool IsServer, int ListenPort) : this(Reciever, IsServer)
        {
            this.Port = ListenPort;
        }

        public override void Start()
        {
            RecieveThread = new Thread(x => Recieve());
            RecieveThread.Start();        
        }

        public override void Close()
        {
            CloseClass = true;
        }

        public void Recieve()
        {
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();

            try
            {
                while (!CloseClass)
                {
                    if (!tcpListener.Pending())
                    {
                        Thread.Sleep(50);
                    }
                    else
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();
                       
                        // SslStream stream = ServerSecureSocket.ManageClientRequest(client);
                        // SslStream stream = SSLTest.ManageServer(client);
                        if (stream != null)
                        {
                            //new Thread(x => ProcessIncomingConnection(stream, client, getEndPoint(client))).Start();
                            ThreadPool.QueueUserWorkItem(x => ProcessIncomingConnection(stream, client, getEndPoint(client)));
                        }
                    }
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private string getEndPoint(TcpClient client)
        {
            try
            {
                return client.Client.RemoteEndPoint.ToString();
            }
            catch
            {
                return "";
            }
        }

        public void ProcessIncomingConnection(Stream stream, TcpClient client, string endPoint)
        {
            try
            {
                Message newMessage = (Message)formatter.Deserialize(stream);
                newMessage.ReturnIP = endPoint.Split(':')[0];
                Reciever.Recieve(newMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Corey found an error here!!!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
    }
}
