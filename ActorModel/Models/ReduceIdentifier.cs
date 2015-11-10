using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Models
{
    internal class ReduceIdentifier
    {
        public Dictionary<long, Dictionary<long, bool>> identifier;
        private long counter = 0;

        public ReduceIdentifier()
        {
            this.identifier = new Dictionary<long, Dictionary<long, bool>>();
        }

        public void CreateNewIdentity(long ID)
        {
            this.identifier.Add(ID, new Dictionary<long, bool>());
        }

        public bool Complete(long ID)
        {
            bool finished = true;
            int count = 0;
            Monitor.Enter(identifier);
            try
            {
                foreach (var item in this.identifier[ID])
                {
                    if (!item.Value)
                    {
                        count++;
                    }
                }
                if(count != 0)
                {
                    finished = false;
                }
            }
            finally
            {
                Monitor.Exit(identifier);
            }
            return finished;
        }

        public void AddNew(long ID, MappedAnswer task)
        {

                if (task.ReduceKey == long.MaxValue)
                {
                    task.ReduceKey = GetNewPlace(ID);
                    this.identifier[ID].Add(task.ReduceKey, false);
                }   

        }

        public bool CheckAndSwitch(long ID, long ReduceID)
        {
            bool isGood = false;
            Monitor.Enter(this.identifier);
            try
            {
                if (!this.identifier[ID][ReduceID])
                {
                    isGood = true;
                    this.identifier[ID][ReduceID] = true;
                }
            }
            finally
            {
                Monitor.Exit(this.identifier);
            }
            
            return isGood;
        }

        public void Remove(long ID)
        {
            Monitor.Enter(this.identifier);
            try
            {
                this.identifier.Remove(ID);
            }
            finally
            {
                Monitor.Exit(this.identifier);
            }
        }

        private long GetNewPlace(long ID)
        {
            Monitor.Enter(this.identifier);
            try
            {
                while (this.identifier[ID].ContainsKey(counter))
                {
                    counter++;
                }
                //foreach (var item in this.identifier[ID])
                //{
                //    counter++;
                //}
            }
            finally
            {
                Monitor.Exit(this.identifier);
            }
            return counter;
        }
    }
}
