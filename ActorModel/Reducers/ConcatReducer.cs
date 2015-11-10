using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class ConcatReducer : IReducer
    {
        private string concat = "";

        public void Reduce(ReduceResult[] RR)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in RR)
            {
                sb.Append(item.Key);
            }

            this.concat = sb.ToString();
        }

        public object getResult()
        {
            return this.concat;
        }
    }
}
