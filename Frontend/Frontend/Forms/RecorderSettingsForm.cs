using System;
using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form contains Property Grid that is used to set which events are supposed to be recorded (RecordedEvents object).
    /// There are also another UI elements for setting RecorderConfiguration object.
    /// </summary>
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
