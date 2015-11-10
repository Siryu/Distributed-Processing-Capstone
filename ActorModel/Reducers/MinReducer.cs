using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class MinReducer : IReducer
    {
        private double Min { get; set; }

        public void Reduce(ReduceResult[] RR)
        {
            double start = 0.0d;
            double.TryParse(RR[0].Key.ToString(), out start);
            Min = start;

            foreach (var item in RR)
            {
                double.TryParse(item.Key.ToString(), out start);
                if(start < Min)
                {
                    Min = start;
                }
            }
        }

        public object getResult()
        {
            return this.Min;
        }
    }
}
