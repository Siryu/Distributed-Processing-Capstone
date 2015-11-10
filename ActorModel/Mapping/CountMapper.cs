using ActorModel.Reducers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Mapping
{
    internal class CountMapper<T> : IMapping where T : IEnumerable
    {
        public MappedAnswer[] Map(object data)
        {
            T setData = (T)data;
            List<MappedAnswer> results = new List<MappedAnswer>();
            foreach (var item in setData)
            {
                bool foundResult = false;
                foreach (var result in results)
                {
                    if(result.Key.Equals(item))
                    {
                        result.Value = (int)result.Value + 1;
                        foundResult = true;
                    }
                }
                if (!foundResult)
                {
                    results.Add(new MappedAnswer((string)item, 1));
                }
            }
            return results.ToArray();
        }
    }
}
