using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class CountReducer : IReducer
    {
        private double count = 0;

        public void Reduce(ReduceResult[] RR)
        {
            this.count = RR.Count();
        }

        public object getResult()
        {
            return this.count;
        }
    }
}
