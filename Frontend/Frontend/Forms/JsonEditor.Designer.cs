namespace Frontend.Forms
{
    partial class JsonEditor
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
            this.modificationsCheckBox = new System.Windows.Forms.CheckBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textEditor
            // 
            this.textEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEditor.Lexer = ScintillaNET.Lexer.Json;
            this.textEditor.Location = new System.Drawing.Point(12, 35);
            this.textEditor.Name = "textEditor";
            this.textEditor.ReadOnly = true;
            this.textEditor.Size = new System.Drawing.Size(776, 403);
            this.textEditor.TabIndex = 0;
            // 
            // modificationsCheckBox
            // 
            this.modificationsCheckBox.AutoSize = true;
            this.modificationsCheckBox.Location = new System.Drawing.Point(12, 10);
            this.modificationsCheckBox.Name = "modificationsCheckBox";
            this.modificationsCheckBox.Size = new System.Drawing.Size(88, 17);
            this.modificationsCheckBox.TabIndex = 1;
            this.modificationsCheckBox.Text = "Modifications";
            this.modificationsCheckBox.UseVisualStyleBackColor = true;
            this.modificationsCheckBox.CheckedChanged += new System.EventHandler(this.modificationsCheckBox_CheckedChanged);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.BackColor = System.Drawing.SystemColors.Control;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Location = new System.Drawing.Point(105, 6);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(234, 24);
            this.warningLabel.TabIndex = 2;
            this.warningLabel.Text = "USE AT YOUR OWN RISK";
            this.warningLabel.Visible = false;
            // 
            // JsonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.modificationsCheckBox);
            this.Controls.Add(this.textEditor);
            this.Name = "JsonEditor";
            this.Text = "JSON Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScintillaNET.Scintilla textEditor;
        private System.Windows.Forms.CheckBox modificationsCheckBox;
        private System.Windows.Forms.Label warningLabel;
    }
}