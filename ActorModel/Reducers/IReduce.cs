using ActorModel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    /// <summary>
    /// Designates how to reduce your mapped items.
    /// </summary>
    public interface IReduce
    {
        /// <summary>
        /// Takes in one or two MappedAnswers and combines them by this method.
        /// </summary>
        /// <param name="mappedData"></param>
        /// <returns>0 or more MappedAnswers.</returns>
        MappedAnswer[] Reduce(MappedAnswer[] mappedData);
    }
}
