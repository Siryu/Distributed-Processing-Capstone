using ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using ActorModel.Interfaces;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using ActorModel.Models;
using ActorModel.Tools;
using ActorModel.Reducers;
using ActorModel.Mapping;

namespace ActorModel.Actors
{
    internal class WorkerActor : Actor
    {
        private Message Message { get; set; }
        private int NetworkTried { get; set; }

        public WorkerActor(Message message)
        {
            this.NetworkTried = 3;
            this.Message = message;
        }

        public bool needsWork()
        {
            bool needsWork = false;
            if(Message == null)
            {
                needsWork = true;
            }
            return needsWork;
        }

        public override void Start()
        {
            //new Thread(x => Work()).Start();
            ThreadPool.QueueUserWorkItem(x => Work());
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        private void Work()
        {
            try
            {
                Message newMessage = null;
                if(Message is MapMessage)
                {
                    MapMessage mm = (MapMessage)Message;
                    newMessage = WorkMapMessage(mm);
                }
                else if(Message is ReduceMessage)
                {
                    ReduceMessage rm = (ReduceMessage)Message;
                    newMessage = WorkReduceMessage(rm);
                }
                new NetworkResponseActor(newMessage, newMessage.Port, newMessage.ReturnIP).Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "Says Corey");
                this.Message = null;
            }
        }

        private MapMessage WorkMapMessage(MapMessage message)
        {
            MapMessage mm = new MapMessage();

            try
            {
                byte[] dll = (byte[])message.MapDLL;
                Assembly assembly = Assembly.Load(dll);
                var type = typeof(IMapping);
                var types = assembly.GetTypes().Where(x => type.IsAssignableFrom(x)).ToArray(); 
                IMapping instanceOfMyType = (IMapping)Activator.CreateInstance(types[0]);
                MappedAnswer[] answer = instanceOfMyType.Map(message.Data);

                mm.Data = answer;
                mm.Piece = message.Piece;
                mm.TaskNumber = message.TaskNumber;
                mm.Port = message.Port;
                mm.ReturnIP = message.ReturnIP;
            }
            finally
            {
                this.Message = null;
            }
            return mm;
        }

        private ReduceAnswerMessage WorkReduceMessage(ReduceMessage message)
        {
            ReduceAnswerMessage ram = new ReduceAnswerMessage();
            try
            {
                Assembly assembly = Assembly.Load(message.ReduceDLL);
                var type = typeof(IReduce);
                var types = assembly.GetTypes().Where(x => type.IsAssignableFrom(x)).ToArray(); 
               
                IReduce instanceOfMyType = (IReduce)Activator.CreateInstance(types[0]);
                MappedAnswer[] answer = instanceOfMyType.Reduce(message.Data);

                if (message.Data[0].isFromMapPart)
                {
                    foreach (var item in answer)
                    {
                        item.isFromMapPart = true;
                    }
                }

                ram.ReduceID = message.ReduceID;
                ram.TaskNumber = message.TaskID;
                ram.Port = message.Port;
                ram.ReturnIP = message.ReturnIP;
                ram.Result = answer;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.Message = null;
            }
            return ram;
        }
    }
}