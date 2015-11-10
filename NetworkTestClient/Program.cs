using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ActorModel.Tools;

namespace NetworkTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("LocalHost", 5099);
            SSLTest.ManageClient(tcpClient);
            Console.ReadLine();
        }
    }
}
