using ActorModel.Models;
using ActorModel.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Client
{
    internal class RemoteServer
    {
        public int ListenPort { get; set; }
        private Thread ListenThread { get; set; }
        private bool CloseClass = false;

        public RemoteServer()
        {
            this.ListenPort = 49701;
        }

        public RemoteServer(int Port)
        {
            this.ListenPort = Port;
        }

        public void Start()
        {
            ListenThread = new Thread(x => Listen());
            ListenThread.Start();
            //ThreadPool.QueueUserWorkItem(x => Listen());
        }

        public void Close()
        {
            //ListenThread.Abort();
            CloseClass = true;
        }

        private void Listen()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, ListenPort);
            tcpListener.Start();
            TcpClient client = null;
            try
            {  
                while (!CloseClass)
                {
                    if (!tcpListener.Pending())
                    {
                        Thread.Yield();
                    }
                    else
                    {
                        client = tcpListener.AcceptTcpClient();
                        ThreadPool.QueueUserWorkItem(x => returnMethod(client));
                    }
                }
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }

                tcpListener.Stop();
            }
        }

        private void returnMethod(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            FileSender fileSender = new FileSender(stream, "./" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".dll");
            fileSender.Start();
   
            stream.Close();
            client.Close();
        }
    }
}
