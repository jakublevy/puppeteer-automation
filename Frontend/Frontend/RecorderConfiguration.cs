using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class RecorderConfiguration
    {
        public PuppeteerOptions PuppeteerOptions { get; set; }
        public RecordedEvents RecordedEvents { get; set; }
    }
}
