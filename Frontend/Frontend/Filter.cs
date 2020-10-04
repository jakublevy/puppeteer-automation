using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class Filter
    {
        //Contains value for hiding

        public List<string> EventTypes { get; set; } = new List<string>();
        public List<string> Targets { get; set; } = new List<string>();
        public List<string> Status { get; set; } = new List<string>();
    }
}
