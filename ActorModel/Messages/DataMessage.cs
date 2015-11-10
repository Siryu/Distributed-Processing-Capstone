using ActorModel.Actors;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Messages
{
    [Serializable]
    internal class DataMessage : Message
    {
        public ReduceResult Data { get; set; }
        public int Place { get; set; }

        public DataMessage()
        {
            this.Data = new ReduceResult();
            this.Port = ManagerActor.LocalPort;
        }
    }
}
