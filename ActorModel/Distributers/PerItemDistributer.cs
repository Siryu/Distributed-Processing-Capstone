using ActorModel.Distributers;
using ActorModel.Reducers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Distributers
{
    internal class PerItemDistributer <T> : IDistributer where T : IEnumerable
    {
        public ReduceResult[] Distribute(object obj)
        {
            List<object> arr = new List<object>();
            T stringO = (T)obj;

            foreach (var item in stringO)
            {
                arr.Add(item);
            }

            ReduceResult[] array = new ReduceResult[arr.Count()];
            int i = 0;
            foreach (var item in arr)
            {
                array[i] = new ReduceResult();
                array[i++].values = item;
            }

            return array;
        }
    }
}
