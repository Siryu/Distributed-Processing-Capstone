using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Mapping
{
    /// <summary>
    /// Item that has been mapped.
    /// </summary>
    [Serializable]
    public class MappedAnswer
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; set; }
        internal bool isWorked = false;
        private DateTime lastSent;
        private long sendWaitTimer = 45000;
        internal long ReduceKey { get; set; }
        internal bool isFromMapPart = false;

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public MappedAnswer()
        {
            lastSent = DateTime.MinValue;
            ReduceKey = long.MaxValue;
        }

        /// <summary>
        /// Constructor taking a key and value.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public MappedAnswer(string Key, object Value) : this()
        {
            this.Key = Key;
            this.Value = Value;
        }

        internal bool ReadyToSend()
        {
            bool ready = false;

            if ((DateTime.Now.Subtract(lastSent)).TotalMilliseconds > sendWaitTimer && !isWorked)
            {
                ready = true;
            }
            
            return ready;
        }

        internal void Sent()
        {
            lastSent = DateTime.Now;
        }
    }
}
