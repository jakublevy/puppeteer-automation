using System;
using System.Reflection;
using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form contains a Property Grid that is used to set properties of CodeGenOptions object.
    /// </summary>
    public partial class CodeGenSettingsForm : Form
    {
        private CodeGenOptions cgo = null;
        public CodeGenSettingsForm()
        {
            InitializeComponent();
            ResizePropertyGridHelpBox();
        }

        public void BindCodeGeneratorOptions(CodeGenOptions c)
        {
            cgo = c;
        }

        public CodeGenOptions ExportCodeGeneratorOptions()
        {
            return cgo;
        }

        private void CodeGeneratorSettings_Load(object sender, EventArgs e)
        {
            codeGeneratorpropertyGrid.SelectedObject = cgo;
        }

        private void CodeGenSettingsForm_Resize(object sender, EventArgs e)
        {
            ResizePropertyGridHelpBox();
        }

        private void ResizePropertyGridHelpBox()
        {
            int newHeight = (int)(0.23 * Height);
            if (newHeight < 95)
            {
                ChangeDescriptionHeight(codeGeneratorpropertyGrid, newHeight);
            }
        }

        /// <summary>
        /// Changes the size of PropertyGrid HelpBox
        /// </summary>
        /// <param name="grid">PropertyGrid whose HelpBox to resize</param>
        /// <param name="height">Requested height of HelpBox</param>
        private static void ChangeDescriptionHeight(PropertyGrid grid, int height)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            foreach (Control control in grid.Controls)
            {
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
        }
    }
}
