using System;
using System.IO;
using System.Windows.Forms;

namespace Frontend.UserControls
{
    /// <summary>
    /// This User control is an UI for configuration of PuppeteerOptions object.
    /// </summary>
    public partial class PuppeteerOptionsUserControl : UserControl
    {
        enum BrowserMode
        {
            Launch, //corresponds to puppeteer.launch(...)
            Connect //corresponds to puppeteer.connect(...)
        }

        public PuppeteerOptionsUserControl()
        {
            InitializeComponent();

            viewportGroupBox.Controls.Remove(viewportEnabledCheckBox);
            Controls.Add(viewportEnabledCheckBox);
            viewportEnabledCheckBox.BringToFront();
        }

        /// <summary>
        /// Fills the UI with respect to the supplied parameter opts.
        /// </summary>
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
            headlessCheckBox.Checked = opts.Headless;

            if (opts is ConnectPuppeteerOptions co)
            {
                hostTextBox.Text = co.EndPoint.Host;
                portTextBox.Text = co.EndPoint.Port.ToString();
                connectionTypeComboBox.SelectedIndex = 1;
                SetBrowserMode(BrowserMode.Connect);
            }
            else if (opts is LaunchPuppeteerOptions lo)
            {
                pathTextBox.Text = lo.ExecutablePath;
                connectionTypeComboBox.SelectedIndex = 0;
                SetBrowserMode(BrowserMode.Launch);
            }
        }

        /// <summary>
        /// Exports current UI into PuppeteerOptions.
        /// </summary>
        /// <returns></returns>
        public PuppeteerOptions ExportOptions()
        {
            PuppeteerOptions po = null;
            if (connectionTypeComboBox.SelectedItem.ToString() == "Connect")
            {
                bool succ = Uri.TryCreate($"http://{hostTextBox.Text}:{portTextBox.Text}", UriKind.Absolute, out var endPoint);
                if (succ)
                {
                    po = new ConnectPuppeteerOptions
                    {
                        EndPoint = endPoint
                    };
                }
                else
                {
                    throw new ArgumentException("IP address or port is invalid. Changes not saved.");
                }


            }
            else if (connectionTypeComboBox.SelectedItem.ToString() == "Launch")
            {
                if (File.Exists(pathTextBox.Text) || pathTextBox.Text == "")
                {
                    po = new LaunchPuppeteerOptions
                    {
                        ExecutablePath = pathTextBox.Text
                    };
                }
                else
                {
                    throw new ArgumentException(
                        "The filled path does not point to the actual file. Changes not saved.");
                }
                
            }

            po.DevTools = devtoolsCheckBox.Checked;
            po.Headless = headlessCheckBox.Checked;
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

        /// <summary>
        /// Setting visibility for controls of launch state (browser path)
        /// </summary>
        private void SetLaunchControlsVisibility(bool state)
        {
            pathTextBox.Visible = state;
            pathLabel.Visible = state;
            browseButton.Visible = state;
        }

        /// <summary>
        /// Setting visibility for controls of connect state (ip address, port)
        /// </summary>
        private void SetConnectControlsVisibility(bool state)
        {
            hostLabel.Visible = state;
            hostTextBox.Visible = state;
            portLabel.Visible = state;
            portTextBox.Visible = state;
        }


        /// <summary>
        /// Sets the UI controls visibility with respect to the given parameter mode.
        /// </summary>
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

        /// <summary>
        /// Changes the current BrowserMode based on the selected value of combo box.
        /// </summary>
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

        /// <summary>
        /// Open file dialog for choosing browser executable path.
        /// </summary>
        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = opfd.FileName;
            }
        }

        /// <summary>
        /// Default connection type is 0 = Launch
        /// </summary>
        private void PuppeteerOptionsUserControl_Load(object sender, EventArgs e)
        {
            connectionTypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// UI logic that enables or disables the viewport group box.
        /// </summary>
        private void viewportEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            viewportGroupBox.Enabled = viewportEnabledCheckBox.Checked;
        }
    }
}
