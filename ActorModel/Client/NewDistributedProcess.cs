using ActorModel.Actors;
using ActorModel.Distributers;
using ActorModel.Mapping;
using ActorModel.Messages;
using ActorModel.Reducers;
using ActorModel.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Client
{
    /// <summary>
    /// Client class to create a Distributed Process.
    /// </summary>
    public class DistributedProcess
    {
        private object Data;
        private string FunctionFile;
        private TcpListener listener;
        private IMapping Mapper;
        private IReducer Reducer;
        private IDistributer Distributer;
        private MappingOptions MapOption;
        private ReduceOptions ReduceOption;
        private DistributerOptions DistributeOption;

        /// <summary>
        /// Standard Constructor taking in required networking credentials.
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        /// <param name="localPort"></param>
        public DistributedProcess(string serverIP, int serverPort, int localPort)
        {
            ManagerActor.ServerIP = serverIP;
            ManagerActor.ServerPort = serverPort;
            ManagerActor.LocalPort = localPort;
            this.MapOption = MappingOptions.NONE;
            this.ReduceOption = ReduceOptions.NONE;
            this.DistributeOption = DistributerOptions.NONE;
        }

        /// <summary>
        /// Begins the distribution process.  Starting all threads and work to be done.
        /// </summary>
        /// <returns>ID to retrieve finished product.</returns>
        public long Start()
        {
            ConnectionTester.TestUntilConnectionMade(ManagerActor.ServerIP, ManagerActor.ServerPort);

            this.listener = new TcpListener(IPAddress.Any, ManagerActor.LocalPort);
            listener.Start();

            SetupWorkMessage message = new SetupWorkMessage();
            message.Data = this.Data;
            message.Port = ManagerActor.LocalPort;
            if(MapOption != MappingOptions.NONE)
            {
                message.mapOption = MapOption;
            }
            if(ReduceOption != ReduceOptions.NONE)
            {
                message.reduceOption = ReduceOption;
            }
            if(DistributeOption != DistributerOptions.NONE)
            {
                message.distributeOption = DistributeOption;
            }
            if(FunctionFile != null && File.Exists(this.FunctionFile))
            {
                message.TheDLL = File.ReadAllBytes(this.FunctionFile);   
            }
            else
            {
                throw new IOException();
            }
            new NetworkResponseActor(message, ManagerActor.ServerPort, ManagerActor.ServerIP).Start();

            return GetWorkID();
        }

        private long GetWorkID()
        {
            TcpClient client = this.listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            DataMessage ram = (DataMessage)GetMessage(stream);
            stream.Close();
            client.Close();
            return (long)ram.Data.values;
        }

        private Message GetMessage(NetworkStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            Message newMessage = (Message)formatter.Deserialize(stream);
            return newMessage;
        }

        /// <summary>
        /// Add work to be done by an object.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>this</returns>
        public DistributedProcess addWork(object Data)
        {
            this.Data = Data;
            return this;
        }

        /// <summary>
        /// Add work to be done as a file.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>this</returns>
        public DistributedProcess addWork(string FileName)
        {
            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException();
            }
            else
            {
                string fileText = File.ReadAllText(FileName);
                this.Data = fileText;
            }
            return this;
        }

        /// <summary>
        /// Retrieves the finished work from the server.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>ReduceResult containing a key and a value.</returns>
        public ReduceResult GetFinishedWork(long ID)
        {
            ReduceResult result = null;
            while (result == null)
            {
                SendRequestForFinishedResult(ID);
                result = GetFinishedResult();
                Thread.Sleep(1000);
            }
            return result;
        }

        private void SendRequestForFinishedResult(long ID)
        {
            DataMessage dm = new DataMessage();
            dm.Data.Key = "Get";
            dm.Data.values = ID;
            dm.Port = ManagerActor.LocalPort;
            dm.Place = -1;
            new NetworkResponseActor(dm, ManagerActor.ServerPort, ManagerActor.ServerIP).Start();
        }

        private ReduceResult GetFinishedResult()
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            IFormatter formatter = new BinaryFormatter();
            DataMessage ram = (DataMessage)formatter.Deserialize(stream);
            stream.Close();
            client.Close();
            return ram.Data;
        }

        /// <summary>
        /// Try to get the finished work from the server, if the work isn't done, continues.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>ReduceResult containing a key and a value.</returns>
        public ReduceResult TryGetFinishedWork(long ID)
        {
            SendRequestForFinishedResult(ID);
            return GetFinishedResult();
        }

        /// <summary>
        /// Adds in the .dll file for Distribute / Map / Reduce
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>this</returns>
        public DistributedProcess AddFunctionFile(string FileName)
        {
            this.FunctionFile = FileName;
            return this;
        }

        /// <summary>
        /// Designates a distribution method to use with your Data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributerOption"></param>
        /// <returns>this</returns>
        public DistributedProcess Distribute<T>(DistributerOptions distributerOption) where T : IEnumerable
        {
            IDistributer distributer = DistributerFactory.CreateDistributer<T>(distributerOption);
            this.Distributer = distributer;
            return this;
        }

        /// <summary>
        /// Designates a mapping method to use with your Data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mappingOption"></param>
        /// <returns>this</returns>
        public DistributedProcess Map<T>(MappingOptions mappingOption) where T : IEnumerable
        {
            IMapping mapper = MappingFactory.CreateMapping<T>(mappingOption);
            this.Mapper = mapper;
            return this;
        }

        /// <summary>
        /// Designates a reduce method to use with your Data.
        /// </summary>
        /// <param name="reduceOption"></param>
        /// <returns>this</returns>
        public DistributedProcess Reduce(ReduceOptions reduceOption)
        {
            IReducer reducer = ReduceFactory.CreateReducer(reduceOption);
            this.Reducer = reducer;
            return this;
        }
    }
}
