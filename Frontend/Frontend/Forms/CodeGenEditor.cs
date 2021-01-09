using System;
using System.Windows.Forms;

namespace Frontend
{
    /*
     * This class uses a text editor ScintillaNET.Scintilla. This code is not my own, it is used with respect to the MIT license.
     * Source code of ScintillaNET project: https://github.com/jacobslusser/ScintillaNET.
     */
    public partial class CodeGenEditor : Form
    {
        public CodeGenEditor()
        {
            InitializeComponent();
        }

        public void SetEditorText(string txt)
        {
            textEditor.Text = txt;
        }

        private void CodeGenEditor_Load(object sender, EventArgs e)
        {
            foreach (var textEditorStyle in textEditor.Styles)
                textEditorStyle.Size = 12;
        }
    }
}
