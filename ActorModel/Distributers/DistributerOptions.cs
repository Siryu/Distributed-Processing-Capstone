using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Distributers
{
    /// <summary>
    /// Options for how to distribute data.
    /// </summary>
    [Serializable]
    public enum DistributerOptions
    {
        /// <summary>
        /// No option
        /// </summary>
        NONE,
        /// <summary>
        /// Set a delimeter then use this option to split by.
        /// </summary>
        PERDELIMETER, 
        /// <summary>
        /// Per line in a text document
        /// </summary>
        PERLINE, 
        /// <summary>
        /// Per item in an enumerable.
        /// </summary>
        PERITEM
    }
}
