using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRSCU
{
    class MoveResponse 
    {
        public int Complete { get; set; }
        public int Failed { get; set; }
        public int Warnings { get; set; }
        public int Remaining { get; set; }
    }
}
