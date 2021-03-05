using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// UI only window showing progress bar with "Waiting for the running action to finish..." message.
    /// </summary>
    public partial class WaitingWindow : Form
    {
        public WaitingWindow()
        {
            InitializeComponent();
        }

        private void WaitingWindow_Load(object sender, System.EventArgs e)
        {
            CenterToParent();
        }

        public void Center()
        {
            CenterToParent();
        }
    }
}
