using ActorModel.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.TaskManager
{
    /// <summary>
    /// Task Manager Class.
    /// </summary>
    public class TaskManager
    {
        RecieverActor processor;
        NetworkRecieverActor nra;

        /// <summary>
        /// Basic Constructor taking in all network items needed.
        /// </summary>
        /// <param name="ServerIP"></param>
        /// <param name="ServerPort"></param>
        /// <param name="ListenPort"></param>
        public TaskManager(string ServerIP, int ServerPort, int ListenPort)
        {
            ManagerActor.LocalPort = ListenPort;
            ManagerActor.ServerIP = ServerIP;
            ManagerActor.ServerPort = ServerPort;
            processor = new Processor();
            nra = new NetworkRecieverActor(processor, false, ManagerActor.LocalPort);
        }

        /// <summary>
        /// Starts the Task Manager threads and networking listeners.
        /// </summary>
        public void Start()
        {
            processor.Start();
            nra.Start();
        }

        /// <summary>
        /// Closes the Task Manager threads and network connections.
        /// </summary>
        public void Close()
        {
            nra.Close();
            processor.Close();
        }
    }
}
