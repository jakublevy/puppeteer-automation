using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class CurrentEdit
    {
        public Thumbnail Thumbnail { get; set; }
        public dynamic StartupHints { get; set; }
        public List<Recording> Recordings { get; set; }
        public Configuration Config { get; set; }
        public int NextAvailableId { get; set; }

        public int AllocateId()
        {
            return NextAvailableId++;
        }
    }
}
