using ActorModel.Reducers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Distributers
{
    /// <summary>
    /// Set how to setup the distribution of data.
    /// </summary>
    public interface IDistributer
    {
        /// <summary>
        /// Takes data object and Convert into pieces.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>ReduceResult[] object broken into different pieces.</returns>
        ReduceResult[] Distribute(object obj);
    }
}
