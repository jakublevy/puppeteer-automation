using System;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;

namespace Frontend.Forms
{
    public partial class JsonEditor : Form
    {
        public JsonEditor()
        {
            InitializeComponent();
            SetJsonFormat();
        }

        public void SetEditorText(string txt)
        {
            bool oldState = textEditor.ReadOnly;
            textEditor.ReadOnly = false;
            textEditor.Text = txt;
            textEditor.ReadOnly = oldState;
        }

        public string GetEditorText()
        {
            return textEditor.Text;
        }

        private void SetJsonFormat()
        {
            textEditor.Styles[Style.Json.Default].ForeColor = Color.Silver;
            textEditor.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            textEditor.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            textEditor.Styles[Style.Json.Number].ForeColor = Color.Olive;
            textEditor.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
            textEditor.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            textEditor.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            textEditor.Styles[Style.Json.Operator].ForeColor = Color.Purple;
            //textEditor.Lexer = Lexer.Json;

            foreach (var textEditorStyle in textEditor.Styles)
                textEditorStyle.Size = 12;
            
            //textEditor.Styles[Style.json]
        }

        private void modificationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            warningLabel.Visible = modificationsCheckBox.Checked;
            textEditor.ReadOnly = !modificationsCheckBox.Checked;
        }
    }
}
