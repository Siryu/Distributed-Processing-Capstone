using ActorModel.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class NodeClient
    {
        static void Main(string[] args)
        {
            Node node = new Node("localhost", 49712, 49702);
            node.Start();
            Console.ReadLine();
            node.Close();
        }
    }
}
