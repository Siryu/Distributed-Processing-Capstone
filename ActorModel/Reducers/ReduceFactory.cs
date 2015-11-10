using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Reducers
{
    internal static class ReduceFactory
    {
        public static IReducer CreateReducer(ReduceOptions option)
        {
            IReducer picked = null;

            switch(option)
            {
                case ReduceOptions.SUM:
                {
                    picked = new SumReducer();
                    break;
                }
                case ReduceOptions.MAX:
                {
                    picked = new MaxReducer();
                    break;
                }
                case ReduceOptions.MIN:
                {
                    picked = new MinReducer();
                    break;
                }
                case ReduceOptions.CONCAT:
                {
                    picked = new ConcatReducer();
                    break;
                }
                case ReduceOptions.AVERAGE:
                {
                    picked = new AverageReducer();
                    break;
                }
                case ReduceOptions.COUNT:
                {
                    picked = new CountReducer();
                    break;
                }
                default:
                {
                    picked = null;
                    break;
                }
            }

            return picked;
        }
    }
}
