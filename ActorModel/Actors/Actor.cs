using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel
{
    internal abstract class Actor
    {
        public abstract void Start();
        public abstract void Close();
    }
}
