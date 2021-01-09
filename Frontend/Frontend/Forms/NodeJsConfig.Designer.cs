namespace Frontend.Forms
{
    partial class NodeJsConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.interpreterTextBox = new System.Windows.Forms.TextBox();
            this.browseInterpreterButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nodeJsEntryPointTextBox = new System.Windows.Forms.TextBox();
            this.browseNodeJsEntryPointButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Node.js Interpreter Location";
            // 
            // interpreterTextBox
            // 
            this.interpreterTextBox.Location = new System.Drawing.Point(12, 25);
            this.interpreterTextBox.Name = "interpreterTextBox";
            this.interpreterTextBox.Size = new System.Drawing.Size(258, 20);
            this.interpreterTextBox.TabIndex = 1;
            this.interpreterTextBox.Text = "node";
            this.interpreterTextBox.TextChanged += new System.EventHandler(this.interpreterTextBox_TextChanged);
            // 
            // browseInterpreterButton
            // 
            this.browseInterpreterButton.Location = new System.Drawing.Point(99, 51);
            this.browseInterpreterButton.Name = "browseInterpreterButton";
            this.browseInterpreterButton.Size = new System.Drawing.Size(75, 23);
            this.browseInterpreterButton.TabIndex = 2;
            this.browseInterpreterButton.Text = "Browse";
            this.browseInterpreterButton.UseVisualStyleBackColor = true;
            this.browseInterpreterButton.Click += new System.EventHandler(this.browseInterpreterButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Backend Entry Point";
            // 
            // nodeJsEntryPointTextBox
            // 
            this.nodeJsEntryPointTextBox.Location = new System.Drawing.Point(12, 128);
            this.nodeJsEntryPointTextBox.Name = "nodeJsEntryPointTextBox";
            this.nodeJsEntryPointTextBox.Size = new System.Drawing.Size(258, 20);
            this.nodeJsEntryPointTextBox.TabIndex = 4;
            this.nodeJsEntryPointTextBox.TextChanged += new System.EventHandler(this.nodeJsEntryPointTextBox_TextChanged);
            // 
            // browseNodeJsEntryPointButton
            // 
            this.browseNodeJsEntryPointButton.Location = new System.Drawing.Point(99, 154);
            this.browseNodeJsEntryPointButton.Name = "browseNodeJsEntryPointButton";
            this.browseNodeJsEntryPointButton.Size = new System.Drawing.Size(75, 23);
            this.browseNodeJsEntryPointButton.TabIndex = 5;
            this.browseNodeJsEntryPointButton.Text = "Browse";
            this.browseNodeJsEntryPointButton.UseVisualStyleBackColor = true;
            this.browseNodeJsEntryPointButton.Click += new System.EventHandler(this.browseNodeJsEntryPointButton_Click);
            // 
            // NodeJsConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 191);
            this.Controls.Add(this.browseNodeJsEntryPointButton);
            this.Controls.Add(this.nodeJsEntryPointTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browseInterpreterButton);
            this.Controls.Add(this.interpreterTextBox);
            this.Controls.Add(this.label1);
            this.Name = "NodeJsConfig";
            this.Text = "NodeJsConfig";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NodeJsConfig_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox interpreterTextBox;
        private System.Windows.Forms.Button browseInterpreterButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nodeJsEntryPointTextBox;
        private System.Windows.Forms.Button browseNodeJsEntryPointButton;
    }
}