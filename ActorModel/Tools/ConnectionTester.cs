using ActorModel.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    internal static class ConnectionTester
    {
        public static void TestUntilConnectionMade(string IPAdress, int port)
        {
            var client = new TcpClient();

            while (!client.Connected)
            {
                var result = client.BeginConnect(IPAdress, port, null, null);

                result.AsyncWaitHandle.WaitOne(500);
                if (!client.Connected)
                {
                    Console.WriteLine("Failed to connect.");
                    Thread.Sleep(5000);
                    continue;
                }
                client.EndConnect(result);

            }
            // we have connected
        }
    }
}
