using System;
using ActorModel.Server;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    public class MainServer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start Server.");
            Console.ReadLine();
            Server p = new Server(49712);
            p.SetAmountOfIPsToReturn(1);
            p.Start();
            Console.ReadLine();
            p.Close();
        }
    }
}
