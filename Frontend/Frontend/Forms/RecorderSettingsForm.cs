using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend.Forms
{
    public partial class RecorderSettingsForm : Form
    {
        public RecorderSettingsForm()
        {
            InitializeComponent();
        }

        private void RecorderSettings_Load(object sender, EventArgs e)
        {
            PuppeteerOptions po = ConfigManager.GetPuppeteerConfiguration();
            RecordedEvents re = ConfigManager.GetRecordedEventsOptions();
            puppeteerOptionsUserControl.BindOptions(po);
            recordedEventsPropertyGrid.SelectedObject = re;
        }


        public RecorderConfiguration ExportRecorderOptions()
        {
            RecorderConfiguration rc = new RecorderConfiguration
            {
                PuppeteerOptions = puppeteerOptionsUserControl.ExportOptions(),
                RecordedEvents = (RecordedEvents) recordedEventsPropertyGrid.SelectedObject
            };
            return rc;
        }
    }
}
