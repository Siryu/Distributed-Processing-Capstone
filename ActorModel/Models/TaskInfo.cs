using ActorModel.Mapping;
using ActorModel.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal abstract class TaskInfo
    {
        public byte[] WorkDLL;
        public ReduceOptions reduceOption;
        public MappingOptions mapOption;
        public string ReturnIP;
        public int ReturnPort;
        public long ID;
        public int taskFinished = 0;

        public TaskInfo()
        {

        }

        public TaskInfo(ReduceOptions reduceOption, MappingOptions mapOption)
        {
            this.reduceOption = reduceOption;
            this.mapOption = mapOption;
        }

        public TaskInfo(long ID, byte[] WorkDll, string ReturnIP, int ReturnPort, ReduceOptions reduceOption = ReduceOptions.NONE, MappingOptions mapOption = MappingOptions.NONE) : this(reduceOption, mapOption)
        {
            this.ID = ID;
            this.WorkDLL = WorkDll;
            this.ReturnIP = ReturnIP;
            this.ReturnPort = ReturnPort;
        }
    }
}
