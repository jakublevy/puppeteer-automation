using Frontend.UserControls;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form is shown when replaying a recording. It contains information about state of replaying and errors.
    /// </summary>
    /// 
    public partial class ReplayViewForm : Form
    {
        //whether close request is currently pending
        private bool closeRequested = false;

        private WaitingWindow ww;
        private readonly CancellationTokenSource cts;
        private readonly EditUserControl euc;

        public ReplayViewForm(EditUserControl e, CancellationTokenSource c)
        {
            InitializeComponent();
            cts = c;
            euc = e;
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

        public void ForceClose()
        {
            replayEndedLabel.Visible = true;
            Close();
        }

        /// <summary>
        /// Adds an error into data grid view
        /// </summary>
        /// <param name="errorMsg">Message to be shown</param>
        /// <param name="id">Id of an action, will be used to create a link between the error and the action</param>
        public void AddError(string errorMsg, int id)
        {
            int rowId = dataGridView.Rows.Add();

            DataGridViewRow row = dataGridView.Rows[rowId];
            row.Cells["idColumn"].Value = id;
            row.Cells["errorsColumn"].Value = errorMsg;

            if (dataGridView.Rows.Count == 1)
            {
                dataGridView.ClearSelection();
            }
        }

        /// <summary>
        /// Sets UI into recording state or idle state (recording has finished).
        /// </summary>
        public void SetRecordingEnded(bool state)
        {
            replayEndedLabel.Visible = state;
            stopButton.Enabled = !state;
        }

        /// <summary>
        /// Notification that the replaying has finished.
        /// This method sets the UI into replaying finished fashion.
        /// </summary>
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

        /// <summary>
        /// On "Stop" button click, request to stop recording.
        /// </summary>
        private void stopButton_Click(object sender, EventArgs e)
        {
            ww = new WaitingWindow { StartPosition = FormStartPosition.CenterParent };
            ww.Show(this);
            cts.Cancel();
        }

        /// <summary>
        /// On error line click, this method notifies EditUserControl to highlight the corresponding action.
        /// </summary>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                clearErrorSelection.Enabled = true;
                DataGridViewRow clickedRow = dataGridView.Rows[e.RowIndex];
                int id = (int)clickedRow.Cells["idColumn"].Value;
                euc.HighlightActionUserControlById(id, Color.FromArgb(206, 32, 41));
            }
        }

        /// <summary>
        /// Removes highlighting caused by a click in the data grid view.
        /// </summary>
        private void clearErrorSelection_Click(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
            euc.ClearErrorCustomColors();
            clearErrorSelection.Enabled = false;
        }

        private void ReplayViewForm_LocationChanged(object sender, EventArgs e)
        {
            ww?.Center();
        }

        private void ReplayViewForm_Resize(object sender, EventArgs e)
        {
            ww?.Center();
        }
    }
}
