using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>

namespace eden4ict
{
    public class counter
    {
        public Counter res = new Counter();
        public Counter ind = new Counter();
        public Counter cap = new Counter();
        public Counter jumper = new Counter();
        public Counter diode = new Counter();
    }

    public class Counter
    {
        public int normal { get; set; }
        public int ed { get; set; }
        public int en { get; set; }
        public int eden { get; set; }
        public int total { get { return ed + en + eden+normal; }}

        public Counter()
        {
            normal = 0;
            ed = 0;
            en = 0;
            eden = 0;
        }
    }
}
