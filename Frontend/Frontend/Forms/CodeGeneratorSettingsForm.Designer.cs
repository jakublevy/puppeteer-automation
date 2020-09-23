namespace Frontend.Forms
{
    partial class CodeGeneratorSettingsForm
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
            this.codeGeneratorpropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.codeGeneratorpropertyGrid.Name = "codeGeneratorpropertyGrid";
            this.codeGeneratorpropertyGrid.Size = new System.Drawing.Size(312, 294);
            this.codeGeneratorpropertyGrid.TabIndex = 0;
            // 
            // CodeGeneratorSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 318);
            this.Controls.Add(this.codeGeneratorpropertyGrid);
            this.Name = "CodeGeneratorSettingsForm";
            this.Text = "CodeGeneratorSettings";
            this.Load += new System.EventHandler(this.CodeGeneratorSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid codeGeneratorpropertyGrid;
    }
}