using System;
using System.Windows.Forms;

namespace Frontend.Forms
{
    public partial class NodeJsConfig : Form
    {
        private NodeJsOptions njc;
        private MainForm mf;
        public NodeJsConfig(MainForm m)
        {
            InitializeComponent();
            mf = m;
        }

        public void BindNodeJsOptions(NodeJsOptions n)
        {
            njc = n;
            interpreterTextBox.Text = njc.InterpreterPath;
            nodeJsEntryPointTextBox.Text = njc.NodeJsEntryPoint;
        }

        public NodeJsOptions ExportNodeJsOptions()
        {
            return njc;
        }
        private void browseInterpreterButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                interpreterTextBox.Text = opfd.FileName;
            }
        }

        private void browseNodeJsEntryPointButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog
            {
                Filter = "JavaScript Files (*.js)|*.js|JavaScript Modules (*.mjs)|*.mjs|All files (*.*)|*.*"
            };
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                nodeJsEntryPointTextBox.Text = opfd.FileName;
            }
        }

        private void interpreterTextBox_TextChanged(object sender, EventArgs e)
        {
            if (njc != null)
            {
                njc.InterpreterPath = interpreterTextBox.Text;
            }
        }

        private void nodeJsEntryPointTextBox_TextChanged(object sender, EventArgs e)
        {
            if (njc != null)
            {
                njc.NodeJsEntryPoint = nodeJsEntryPointTextBox.Text;
            }
        }

        private void NodeJsConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            mf.PerformSettingsChecks();
        }
    }
}
