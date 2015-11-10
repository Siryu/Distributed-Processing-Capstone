using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Mapping
{
    /// <summary>
    /// Mapping options
    /// </summary>
    [Serializable]
    public enum MappingOptions
    {
        /// <summary>
        /// No option
        /// </summary>
        NONE, 
        /// <summary>
        /// Mapped by the count of items.
        /// </summary>
        COUNT
    }
}
