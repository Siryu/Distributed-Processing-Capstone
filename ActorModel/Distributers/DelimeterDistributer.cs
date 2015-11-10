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
    internal class DelimeterDistributer<T> : IDistributer where T : IEnumerable
    {
        private string[] Delimeter;

        public ReduceResult[] Distribute(object obj)
        {
            string input = (string)obj;
            string[] splitInput = input.Split(Delimeter, StringSplitOptions.None);
            ReduceResult[] splitWork = new ReduceResult[splitInput.Length];
            for (int i = 0; i < splitWork.Length; i++)
            {
                splitWork[i] = new ReduceResult(null, splitInput[i]);
            }
            return splitWork;
        }

        public void SetDelimeter(string[] Delimeter)
        {
            this.Delimeter = Delimeter;
        }
    }
}
