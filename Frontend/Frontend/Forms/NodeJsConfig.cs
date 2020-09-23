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
    public partial class NodeJsConfig : Form
    {
        private NodeJsOptions njc;
        public NodeJsConfig()
        {
            InitializeComponent();
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
    }
}
