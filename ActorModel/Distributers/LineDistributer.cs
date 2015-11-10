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
    class LineDistributer<T> : DelimeterDistributer<T>, IDistributer where T : IEnumerable
    {
        public LineDistributer()
        {
            SetDelimeter(new string[] {Environment.NewLine});
        }
    }
}
