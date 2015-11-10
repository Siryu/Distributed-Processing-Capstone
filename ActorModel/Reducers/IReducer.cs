using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    /// <summary>
    /// used to retrieve the final answer.
    /// </summary>
    public interface IReducer
    {
        /// <summary>
        /// final reduce answers.
        /// </summary>
        /// <param name="RR"></param>
        void Reduce(ReduceResult[] RR);
        /// <summary>
        /// pulls the ReduceResult from the method.
        /// </summary>
        /// <returns></returns>
        object getResult();
    }
}
