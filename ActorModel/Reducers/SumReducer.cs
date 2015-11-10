using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal class SumReducer : IReducer
    {
        private double Sum { get; set; }

        public void Reduce(ReduceResult[] RR)
        {
            double total = 0;
            foreach (var item in RR)
            {
                double key = 0;
                double.TryParse(item.Key.ToString(), out key);
                total += key;
            }
            this.Sum = total;
        }

        public object getResult()
        {
            return this.Sum;
        }
    }
}
