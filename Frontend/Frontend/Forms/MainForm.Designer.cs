namespace Frontend
{
    partial class MainForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recorderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodejsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewRecordingButton = new System.Windows.Forms.Button();
            this.thumbnailsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.websitesLabel = new System.Windows.Forms.Label();
            this.createdLabel = new System.Windows.Forms.Label();
            this.updatedLabel = new System.Windows.Forms.Label();
            this.editUserControl = new Frontend.UserControls.EditUserControl();
            this.nodeInterpreterVersionLabel = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(831, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recorderToolStripMenuItem,
            this.codeGeneratorToolStripMenuItem,
            this.playerToolStripMenuItem,
            this.nodejsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // recorderToolStripMenuItem
            // 
            this.recorderToolStripMenuItem.Name = "recorderToolStripMenuItem";
            this.recorderToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.recorderToolStripMenuItem.Text = "Recorder and Browser";
            this.recorderToolStripMenuItem.Click += new System.EventHandler(this.recorderToolStripMenuItem_Click);
            // 
            // codeGeneratorToolStripMenuItem
            // 
            this.codeGeneratorToolStripMenuItem.Name = "codeGeneratorToolStripMenuItem";
            this.codeGeneratorToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.codeGeneratorToolStripMenuItem.Text = "Code Generator";
            this.codeGeneratorToolStripMenuItem.Click += new System.EventHandler(this.codeGeneratorToolStripMenuItem_Click);
            // 
            // playerToolStripMenuItem
            // 
            this.playerToolStripMenuItem.Name = "playerToolStripMenuItem";
            this.playerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.playerToolStripMenuItem.Text = "Player ";
            this.playerToolStripMenuItem.Click += new System.EventHandler(this.playerToolStripMenuItem_Click);
            // 
            // nodejsToolStripMenuItem
            // 
            this.nodejsToolStripMenuItem.Name = "nodejsToolStripMenuItem";
            this.nodejsToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.nodejsToolStripMenuItem.Text = "Node.js";
            this.nodejsToolStripMenuItem.Click += new System.EventHandler(this.nodejsToolStripMenuItem_Click);
            // 
            // addNewRecordingButton
            // 
            this.addNewRecordingButton.Enabled = false;
            this.addNewRecordingButton.Location = new System.Drawing.Point(12, 27);
            this.addNewRecordingButton.Name = "addNewRecordingButton";
            this.addNewRecordingButton.Size = new System.Drawing.Size(94, 23);
            this.addNewRecordingButton.TabIndex = 1;
            this.addNewRecordingButton.Text = "Add Recording";
            this.addNewRecordingButton.UseVisualStyleBackColor = true;
            this.addNewRecordingButton.Click += new System.EventHandler(this.addNewRecordingButton_Click);
            // 
            // thumbnailsFlowLayoutPanel
            // 
            this.thumbnailsFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thumbnailsFlowLayoutPanel.Enabled = false;
            this.thumbnailsFlowLayoutPanel.Location = new System.Drawing.Point(12, 87);
            this.thumbnailsFlowLayoutPanel.Name = "thumbnailsFlowLayoutPanel";
            this.thumbnailsFlowLayoutPanel.Size = new System.Drawing.Size(807, 336);
            this.thumbnailsFlowLayoutPanel.TabIndex = 2;
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(12, 57);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(53, 23);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name";
            // 
            // websitesLabel
            // 
            this.websitesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.websitesLabel.Location = new System.Drawing.Point(139, 57);
            this.websitesLabel.Name = "websitesLabel";
            this.websitesLabel.Size = new System.Drawing.Size(100, 23);
            this.websitesLabel.TabIndex = 4;
            this.websitesLabel.Text = "Websites";
            // 
            // createdLabel
            // 
            this.createdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdLabel.Location = new System.Drawing.Point(274, 57);
            this.createdLabel.Name = "createdLabel";
            this.createdLabel.Size = new System.Drawing.Size(100, 23);
            this.createdLabel.TabIndex = 5;
            this.createdLabel.Text = "Created";
            // 
            // updatedLabel
            // 
            this.updatedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatedLabel.Location = new System.Drawing.Point(394, 57);
            this.updatedLabel.Name = "updatedLabel";
            this.updatedLabel.Size = new System.Drawing.Size(100, 23);
            this.updatedLabel.TabIndex = 6;
            this.updatedLabel.Text = "Updated";
            // 
            // editUserControl
            // 
            this.editUserControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editUserControl.Location = new System.Drawing.Point(15, 7);
            this.editUserControl.Name = "editUserControl";
            this.editUserControl.WorkingState = Frontend.UserControls.EditUserControl.State.Disconnected;
            this.editUserControl.Size = new System.Drawing.Size(804, 415);
            this.editUserControl.TabIndex = 7;
            this.editUserControl.Visible = false;
            // 
            // nodeInterpreterVersionLabel
            // 
            this.nodeInterpreterVersionLabel.AutoSize = true;
            this.nodeInterpreterVersionLabel.Location = new System.Drawing.Point(70, 6);
            this.nodeInterpreterVersionLabel.Name = "nodeInterpreterVersionLabel";
            this.nodeInterpreterVersionLabel.Size = new System.Drawing.Size(49, 13);
            this.nodeInterpreterVersionLabel.TabIndex = 8;
            this.nodeInterpreterVersionLabel.Text = "node -v: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 434);
            this.Controls.Add(this.nodeInterpreterVersionLabel);
            this.Controls.Add(this.editUserControl);
            this.Controls.Add(this.updatedLabel);
            this.Controls.Add(this.createdLabel);
            this.Controls.Add(this.websitesLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.thumbnailsFlowLayoutPanel);
            this.Controls.Add(this.addNewRecordingButton);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Puppeteer Recorder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recorderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerToolStripMenuItem;
        private System.Windows.Forms.Button addNewRecordingButton;
        private System.Windows.Forms.FlowLayoutPanel thumbnailsFlowLayoutPanel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label websitesLabel;
        private System.Windows.Forms.Label createdLabel;
        private System.Windows.Forms.Label updatedLabel;
        private System.Windows.Forms.ToolStripMenuItem nodejsToolStripMenuItem;
        private UserControls.EditUserControl editUserControl;
        private System.Windows.Forms.Label nodeInterpreterVersionLabel;
    }
}

