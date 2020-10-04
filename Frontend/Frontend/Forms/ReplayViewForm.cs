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
    public partial class ReplayViewForm : Form
    {
        public ReplayViewForm()
        {
            InitializeComponent();
        }

        public void AddError(string errorMsg)
        {
            int rowId = dataGridView.Rows.Add();

            DataGridViewRow row = dataGridView.Rows[rowId];
            row.Cells["errorsColumn"].Value = errorMsg;
        }

        public void SetRecordingEndedVisibility(bool state)
        {
            replayEndedLabel.Visible = state;
        }
    }
}
