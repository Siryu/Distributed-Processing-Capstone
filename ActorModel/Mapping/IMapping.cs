using ActorModel.Reducers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Mapping
{
    /// <summary>
    /// Used to determine how to map your data.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Takes your distributed item and returns 0 or more MappedAnswers
        /// </summary>
        /// <param name="data"></param>
        /// <returns>MappedAnswer[] with 0 or more MappedAnswers</returns>
        MappedAnswer[] Map(object data);
    }
}
