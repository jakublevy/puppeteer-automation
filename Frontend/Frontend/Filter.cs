using System.Collections.Generic;

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
