namespace Frontend
{
    partial class CodeGenEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textEditor = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // textEditor
            // 
            this.textEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEditor.Lexer = ScintillaNET.Lexer.Cpp;
            this.textEditor.Location = new System.Drawing.Point(12, 12);
            this.textEditor.Name = "textEditor";
            this.textEditor.Size = new System.Drawing.Size(776, 426);
            this.textEditor.TabIndex = 0;
            // 
            // CodeGenEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textEditor);
            this.Name = "CodeGenEditor";
            this.Text = "Code Editor";
            this.Load += new System.EventHandler(this.CodeGenEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla textEditor;
    }
}