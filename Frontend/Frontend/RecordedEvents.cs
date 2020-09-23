using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend
{
    public class RecordedEvents
    {
        public bool Click { get; set; } = true;
        public bool DblClick { get; set; } = true;
        public bool Change { get; set; } = true;
        public bool Input { get; set; } = true;
        public bool Select { get; set; } = true;
        public bool Submit { get; set; } = true;
        public bool Scroll { get; set; } = true;
        public bool Copy { get; set; } = true;
        public bool Paste { get; set; } = true;

        public bool PageClosed { get; set; } = true;
        public bool PageSwitched { get; set; } = true;
        public bool PageOpened { get; set; } = true;
        public bool PageUrlChanged { get; set; } = true;

        [Description("Press h while hovering when in browser to record")]
        public bool MouseOver { get; } = true;
    }
}
