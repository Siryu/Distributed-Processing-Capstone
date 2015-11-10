using ActorModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5099);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            SSLTest.ManageServer(client);
            Console.ReadLine();
        }
    }
}
