using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class AverageReducer : IReducer
    {
        private double Average { get; set; }

        public void Reduce(ReduceResult[] RR)
        {
            double total = 0d;
            int count = 0;

            foreach(var item in RR)
            {
                double key = 0;
                double.TryParse(item.Key.ToString(), out key);
                total += key;
                count++;
            }

            this.Average = total / (double)count;
        }

        public object getResult()
        {
            return this.Average;
        }
    }
}
