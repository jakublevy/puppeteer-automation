using System;
using System.Windows.Forms;

namespace Frontend
{
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
