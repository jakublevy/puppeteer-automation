using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frontend.UserControls;

namespace Frontend.Forms
{
    public partial class ReplayViewForm : Form
    {
        private bool closeRequested = false;
        private WaitingWindow ww;
        private CancellationTokenSource cts;
        private EditUserControl euc;
        public ReplayViewForm(EditUserControl e, CancellationTokenSource c)
        {
            InitializeComponent();
            cts = c;
            euc = e;
        }

        public void AddError(string errorMsg, int id)
        {
            int rowId = dataGridView.Rows.Add();

            DataGridViewRow row = dataGridView.Rows[rowId];
            row.Cells["idColumn"].Value = id;
            row.Cells["errorsColumn"].Value = errorMsg;

            if(dataGridView.Rows.Count == 1)
                dataGridView.ClearSelection();
        }

        public void SetRecordingEnded(bool state)
        {
            replayEndedLabel.Visible = state;
            stopButton.Enabled = !state;
        }

        public void ReplayEndedNow()
        {
            ww?.Close();
            SetRecordingEnded(true);

            if (closeRequested)
            {
                euc.ClearAucCustomColors();
                Close();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            ww = new WaitingWindow();
            ww.Show();
            cts.Cancel();
        }

        private void ReplayViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!replayEndedLabel.Visible)
            {
                DialogResult dr = MessageBox.Show("The replay is still running. Do you want to cancel replaying?", "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    closeRequested = true;
                    stopButton.PerformClick();
                }

                e.Cancel = true;
            }
            else
            {
                euc.ClearAucCustomColors();
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clearErrorSelection.Enabled = true;
            DataGridViewRow clickedRow = dataGridView.Rows[e.RowIndex];
            int id = (int) clickedRow.Cells["idColumn"].Value;
            euc.HighlightActionUserControlById(id, Color.FromArgb(206,32,41));
        }

        private void clearErrorSelection_Click(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
            euc.ClearErrorCustomColors();
            clearErrorSelection.Enabled = false;
        }

        public void ForceClose()
        {
            replayEndedLabel.Visible = true;
            Close();
        }
    }
}
