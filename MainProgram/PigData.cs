using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProgram
{
    [Serializable]
    class PigData
    {
        public int generations = 0;
        public double distanceToPig = 0;
        public double wallheight = 0;
        public double maximumforce = 0;
    }
}
