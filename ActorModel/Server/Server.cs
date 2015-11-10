using ActorModel.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Server
{
    /// <summary>
    /// Server library used to create a Server.
    /// </summary>
    public class Server
    {
        RecieverActor processor;
        NetworkRecieverActor nra;

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="ListenPort"></param>
        public Server(int ListenPort)
        {
            ManagerActor.LocalPort = ListenPort;
            processor = new Processor();
            nra = new NetworkRecieverActor(processor, true, ManagerActor.LocalPort);
        }

        /// <summary>
        /// Starts the Server threads and network listeners.
        /// </summary>
        public void Start()
        {
            processor.Start();
            nra.Start();
        }

        /// <summary>
        /// Closes the Server threads and network connections.
        /// </summary>
        public void Close()
        {
            nra.Close();
            processor.Close();
        }

        /// <summary>
        /// Sets the amount of IP's the server returns on each request.
        /// </summary>
        /// <param name="amount"></param>
        public void SetAmountOfIPsToReturn(int amount)
        {
            Processor p = null;
            if(processor is Processor)
            {
                p = (Processor)processor;
                p.ReturnAmount = amount;
            }
        }
    }
}
