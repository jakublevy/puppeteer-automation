namespace Frontend.Forms
{
    partial class PlayerForm
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
            this.puppeteerOptionsUserControl = new Frontend.UserControls.PuppeteerOptionsUserControl();
            this.codeGeneratorOptionsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // puppeteerOptionsUserControl
            // 
            this.puppeteerOptionsUserControl.Location = new System.Drawing.Point(12, 12);
            this.puppeteerOptionsUserControl.Name = "puppeteerOptionsUserControl";
            this.puppeteerOptionsUserControl.Size = new System.Drawing.Size(236, 290);
            this.puppeteerOptionsUserControl.TabIndex = 0;
            // 
            // codeGeneratorOptionsPropertyGrid
            // 
            this.codeGeneratorOptionsPropertyGrid.Location = new System.Drawing.Point(278, 12);
            this.codeGeneratorOptionsPropertyGrid.Name = "codeGeneratorOptionsPropertyGrid";
            this.codeGeneratorOptionsPropertyGrid.Size = new System.Drawing.Size(191, 284);
            this.codeGeneratorOptionsPropertyGrid.TabIndex = 1;
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 308);
            this.Controls.Add(this.codeGeneratorOptionsPropertyGrid);
            this.Controls.Add(this.puppeteerOptionsUserControl);
            this.Name = "PlayerForm";
            this.Text = "PlayerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.PuppeteerOptionsUserControl puppeteerOptionsUserControl;
        private System.Windows.Forms.PropertyGrid codeGeneratorOptionsPropertyGrid;
    }
}