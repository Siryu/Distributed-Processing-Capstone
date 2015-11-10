using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActorModel.Actors;
using ActorModel.Messages;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using ActorModel.Models;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ActorModel;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // port 49712 == server
            // port 49710 == client



            // :::::::SERVER TEST::::::::
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(x => ServerTest());
            }
            Console.ReadLine();
        }

        private static void ServerTest()
        {
            OkToWorkActor ok = new OkToWorkActor("localhost", 49712, 49710);
            ok.Start();
        }
    }
}
