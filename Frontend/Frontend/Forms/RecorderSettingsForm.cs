using System;
using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form contains a Property Grid that is used to set which events are supposed to be recorded (RecordedEvents object).
    /// There are also other UI elements for setting RecorderConfiguration object.
    /// </summary>
    public partial class RecorderSettingsForm : Form
    {
        public RecorderSettingsForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.icon;
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
            RecordedEvents r = (RecordedEvents)recordedEventsPropertyGrid.SelectedObject;
            try
            {
                PuppeteerOptions p = puppeteerOptionsUserControl.ExportOptions();
                RecorderConfiguration rc = new RecorderConfiguration
                {
                    PuppeteerOptions = p,
                    RecordedEvents = r
                };
                return rc;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new RecorderConfiguration
                {
                    PuppeteerOptions = ConfigManager.GetPuppeteerConfiguration(),
                    RecordedEvents = r
                };
            }
        }
    }
}
