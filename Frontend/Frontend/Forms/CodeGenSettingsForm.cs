using System;
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
    }
}
