using ActorModel.Distributers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Distributers
{
    internal static class DistributerFactory
    {
        public static IDistributer CreateDistributer<T>(DistributerOptions distributerOption) where T : IEnumerable
        {
            IDistributer chosenDistributer = null;
            switch (distributerOption)
            {
                case DistributerOptions.PERITEM:
                    {
                        chosenDistributer = new PerItemDistributer<T>();
                        break;
                    }
                case DistributerOptions.PERDELIMETER:
                    {
                        chosenDistributer = new DelimeterDistributer<T>();
                        break;
                    }
                case DistributerOptions.PERLINE:
                    {
                        chosenDistributer = new LineDistributer<T>();
                        break;
                    }
            }

            return chosenDistributer;
        }
    }
}
