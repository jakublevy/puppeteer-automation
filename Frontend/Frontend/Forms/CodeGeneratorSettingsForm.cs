using System;
using System.Windows.Forms;

namespace Frontend.Forms
{
    /// <summary>
    /// This form contains a Property Grid that is used to set properties of CodeGeneratorOptions object.
    /// </summary>
    public partial class CodeGeneratorSettingsForm : Form
    {
        private CodeGeneratorOptions cgo = null;
        public CodeGeneratorSettingsForm()
        {
            InitializeComponent();
        }

        public void BindCodeGeneratorOptions(CodeGeneratorOptions c)
        {
            cgo = c;
        }

        public CodeGeneratorOptions ExportCodeGeneratorOptions()
        {
            return cgo;
        }

        private void CodeGeneratorSettings_Load(object sender, EventArgs e)
        {
            codeGeneratorpropertyGrid.SelectedObject = cgo;
        }
    }
}
