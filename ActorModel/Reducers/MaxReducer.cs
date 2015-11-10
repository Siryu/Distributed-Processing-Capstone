using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class MaxReducer : IReducer
    {
        private double Max { get; set; }

        public void Reduce(ReduceResult[] RR)
        {
            double start = 0.0d;
            double.TryParse(RR[0].Key.ToString(), out start);
            Max = start;
            foreach (var item in RR)
            {
                double.TryParse(item.Key.ToString(), out start);
                if(start > Max)
                {
                    Max = start;
                }
            }
        }

        public object getResult()
        {
            return this.Max;
        }
    }
}
