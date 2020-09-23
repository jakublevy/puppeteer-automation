using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            textEditor.Text = txt;
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

            textEditor.Text = "prdel";

            foreach (var textEditorStyle in textEditor.Styles)
                textEditorStyle.Size = 12;
            
            //textEditor.Styles[Style.json]
        }
    }
}
