using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Mapping
{
    internal static class MappingFactory
    {
        public static IMapping CreateMapping<T>(MappingOptions option) where T : IEnumerable
        {
            IMapping chosenMapping = null;
            switch(option)
            {
                case MappingOptions.COUNT:
                    {
                        chosenMapping = new CountMapper<T>();
                        break;
                    }
            }
            return chosenMapping;
        }
    }
}
