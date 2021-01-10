using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form contains a Property Grid that is used to set properties of PlayerOptions object.
    /// </summary>
    public partial class PlayerForm : Form
    {
        private PlayerOptions po;
        public PlayerForm()
        {
            InitializeComponent();
        }

        public PlayerOptions ExportOptions()
        {
            return po;
        }

        public void BindOptions(PlayerOptions p)
        {
            po = p;
            codeGeneratorOptionsPropertyGrid.SelectedObject = po;
        }
    }
}
