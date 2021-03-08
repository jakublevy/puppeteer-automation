using System;
using System.Reflection;
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
            ResizePropertyGridHelpBox();
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

        private void ResizePropertyGridHelpBox()
        {
            int newHeight = (int)(0.37 * Height);
            if (newHeight < 95)
                ChangeDescriptionHeight(codeGeneratorOptionsPropertyGrid, newHeight);
        }

        /// <summary>
        /// Changes the size of PropertyGrid HelpBox
        /// </summary>
        /// <param name="grid">PropertyGrid whose HelpBox to resize</param>
        /// <param name="height">Requested height of HelpBox</param>
        private static void ChangeDescriptionHeight(PropertyGrid grid, int height)
        {
            if (grid == null) throw new ArgumentNullException("grid");

            foreach (Control control in grid.Controls)
                if (control.GetType().Name == "DocComment")
                {
                    FieldInfo fieldInfo = control.GetType().BaseType.GetField("userSized",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);
                    fieldInfo.SetValue(control, true);
                    control.Height = height;
                    return;
                }
        }

        private void codeGeneratorOptionsPropertyGrid_Resize(object sender, EventArgs e)
        {
            ResizePropertyGridHelpBox();
        }
    }
}
