using ActorModel.TaskManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerNode
{
    public class TaskManagerNode
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start Task Manager.");
            Console.ReadLine();
            TaskManager tm = new TaskManager("localhost", 49712, 56000);
            tm.Start();
            Console.ReadLine();
            tm.Close();
        }
    }
}
