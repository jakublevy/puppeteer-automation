namespace Frontend.Forms
{
    partial class CodeGenSettingsForm
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
            this.codeGeneratorpropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // codeGeneratorpropertyGrid
            // 
            this.codeGeneratorpropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeGeneratorpropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.codeGeneratorpropertyGrid.Name = "codeGeneratorpropertyGrid";
            this.codeGeneratorpropertyGrid.Size = new System.Drawing.Size(312, 338);
            this.codeGeneratorpropertyGrid.TabIndex = 0;
            // 
            // CodeGenSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 362);
            this.Controls.Add(this.codeGeneratorpropertyGrid);
            this.Name = "CodeGenSettingsForm";
            this.Text = "CodeGeneratorSettings";
            this.Load += new System.EventHandler(this.CodeGeneratorSettings_Load);
            this.Resize += new System.EventHandler(this.CodeGenSettingsForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid codeGeneratorpropertyGrid;
    }
}