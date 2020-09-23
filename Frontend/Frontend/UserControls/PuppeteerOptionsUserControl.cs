using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend.UserControls
{
    public partial class PuppeteerOptionsUserControl : UserControl
    {
        enum BrowserMode
        {
            Launch, Connect
        }

        public PuppeteerOptionsUserControl()
        {
            InitializeComponent();

            viewportGroupBox.Controls.Remove(viewportEnabledCheckBox);
            Controls.Add(viewportEnabledCheckBox);
            viewportEnabledCheckBox.BringToFront();
        }

        public void BindOptions(PuppeteerOptions opts)
        {
            if (opts.Viewport != null)
            {
                widthNumericUpDown.Value = opts.Viewport.Width;
                heightNumericUpDown.Value = opts.Viewport.Height;
                scaleNumericUpDown.Value = Convert.ToDecimal(opts.Viewport.DeviceScaleFactor);
                mobileCheckBox.Checked = opts.Viewport.IsMobile;
                touchCheckBox.Checked = opts.Viewport.HasTouch;
                landscapeCheckBox.Checked = opts.Viewport.IsLandscape;
            }
            viewportEnabledCheckBox.Checked = opts.Viewport != null;
            slowMoNumericUpDown.Value = opts.SlowMo;
            devtoolsCheckBox.Checked = opts.DevTools;

            if (opts is ConnectPuppeteerOptions co)
            {
                hostTextBox.Text = co.EndPoint.Host;
                portTextBox.Text = co.EndPoint.Port.ToString();
                SetBrowserMode(BrowserMode.Connect);
            }
            else if (opts is LaunchPuppeteerOptions lo)
            {
                pathTextBox.Text = lo.ExecutablePath;
                SetBrowserMode(BrowserMode.Launch);
            }
        }

        public PuppeteerOptions ExportOptions()
        {
            PuppeteerOptions po = null;
            if (connectionTypeComboBox.SelectedItem.ToString() == "Connect")
            {
                po = new ConnectPuppeteerOptions
                {
                    EndPoint = new Uri($"http://{hostTextBox.Text}:{portTextBox.Text}")
                };

            }
            else if (connectionTypeComboBox.SelectedItem.ToString() == "Launch")
            {
                po = new LaunchPuppeteerOptions
                {
                    ExecutablePath = pathTextBox.Text
                };
            }

            po.DevTools = devtoolsCheckBox.Checked;
            po.SlowMo = Decimal.ToInt32(slowMoNumericUpDown.Value);

            if (viewportEnabledCheckBox.Checked)
            {
                po.Viewport = new Viewport
                {
                    DeviceScaleFactor = Convert.ToDouble(scaleNumericUpDown.Value), HasTouch = touchCheckBox.Checked,
                    Height = Decimal.ToInt32(heightNumericUpDown.Value),
                    Width = Decimal.ToInt32(widthNumericUpDown.Value),
                    IsLandscape = landscapeCheckBox.Checked, IsMobile = mobileCheckBox.Checked
                };
            }
            else
            {
                po.Viewport = null;
            }

            return po;
        }

        private void SetLaunchControlsVisibility(bool state)
        {
            pathTextBox.Visible = state;
            pathLabel.Visible = state;
            browseButton.Visible = state;
        }

        private void SetConnectControlsVisibility(bool state)
        {
            hostLabel.Visible = state;
            hostTextBox.Visible = state;
            portLabel.Visible = state;
            portTextBox.Visible = state;
        }

        private void SetBrowserMode(BrowserMode mode)
        {
            if (mode == BrowserMode.Launch)
            {
                SetLaunchControlsVisibility(true);
                SetConnectControlsVisibility(false);
            }
            else if (mode == BrowserMode.Connect)
            {
                SetLaunchControlsVisibility(false);
                SetConnectControlsVisibility(true);
            }
        }

        private void connectionTypeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (connectionTypeComboBox.SelectedItem.ToString() == "Connect")
            {
                SetBrowserMode(BrowserMode.Connect);
            }
            else if (connectionTypeComboBox.SelectedItem.ToString() == "Launch")
            {
                SetBrowserMode(BrowserMode.Launch);
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = opfd.FileName;
            }
        }

        private void PuppeteerOptionsUserControl_Load(object sender, EventArgs e)
        {
            connectionTypeComboBox.SelectedIndex = 0;
        }

        private void viewportEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            viewportGroupBox.Enabled = viewportEnabledCheckBox.Checked;
        }
    }
}
