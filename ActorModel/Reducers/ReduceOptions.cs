using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    /// <summary>
    /// Reduce options for mapped data.
    /// </summary>
    [Serializable]
    public enum ReduceOptions
    {
        /// <summary>
        /// No option.
        /// </summary>
        NONE, 
        /// <summary>
        /// Sum all Values
        /// </summary>
        SUM,
        /// <summary>
        /// Find the max of the Values
        /// </summary>
        MAX,
        /// <summary>
        /// Concatinate all the values together.
        /// </summary>
        CONCAT, 
        /// <summary>
        /// Find the Average of the Values
        /// </summary>
        AVERAGE, 
        /// <summary>
        /// Count the Values.
        /// </summary>
        COUNT, 
        /// <summary>
        /// Find the minimum of the Values.
        /// </summary>
        MIN
    }
}
