using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    /// <summary>
    /// Key Value Pair
    /// </summary>
    [Serializable]
    public class ReduceResult
    {
        /// <summary>
        /// Key
        /// </summary>
        public object Key { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public object values { get; set; }

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public ReduceResult()
        {

        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="values"></param>
        public ReduceResult(object Key, object values)
        {
            this.Key = Key;
            this.values = values;
        }
    }
}
