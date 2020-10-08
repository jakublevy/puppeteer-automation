namespace Frontend.UserControls
{
    partial class PuppeteerOptionsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewportGroupBox = new System.Windows.Forms.GroupBox();
            this.viewportEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.scaleNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.landscapeCheckBox = new System.Windows.Forms.CheckBox();
            this.touchCheckBox = new System.Windows.Forms.CheckBox();
            this.mobileCheckBox = new System.Windows.Forms.CheckBox();
            this.heightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.widthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.connectionTypeComboBox = new System.Windows.Forms.ComboBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.hostLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.devtoolsCheckBox = new System.Windows.Forms.CheckBox();
            this.slowMoLabel = new System.Windows.Forms.Label();
            this.slowMoNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.headlessCheckBox = new System.Windows.Forms.CheckBox();
            this.viewportGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slowMoNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // viewportGroupBox
            // 
            this.viewportGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewportGroupBox.Controls.Add(this.viewportEnabledCheckBox);
            this.viewportGroupBox.Controls.Add(this.scaleNumericUpDown);
            this.viewportGroupBox.Controls.Add(this.label3);
            this.viewportGroupBox.Controls.Add(this.landscapeCheckBox);
            this.viewportGroupBox.Controls.Add(this.touchCheckBox);
            this.viewportGroupBox.Controls.Add(this.mobileCheckBox);
            this.viewportGroupBox.Controls.Add(this.heightNumericUpDown);
            this.viewportGroupBox.Controls.Add(this.label2);
            this.viewportGroupBox.Controls.Add(this.label1);
            this.viewportGroupBox.Controls.Add(this.widthNumericUpDown);
            this.viewportGroupBox.Location = new System.Drawing.Point(3, 3);
            this.viewportGroupBox.Name = "viewportGroupBox";
            this.viewportGroupBox.Size = new System.Drawing.Size(230, 123);
            this.viewportGroupBox.TabIndex = 0;
            this.viewportGroupBox.TabStop = false;
            this.viewportGroupBox.Text = "Viewport";
            // 
            // viewportEnabledCheckBox
            // 
            this.viewportEnabledCheckBox.AutoSize = true;
            this.viewportEnabledCheckBox.Checked = true;
            this.viewportEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewportEnabledCheckBox.Location = new System.Drawing.Point(58, 2);
            this.viewportEnabledCheckBox.Name = "viewportEnabledCheckBox";
            this.viewportEnabledCheckBox.Size = new System.Drawing.Size(65, 17);
            this.viewportEnabledCheckBox.TabIndex = 9;
            this.viewportEnabledCheckBox.Text = "Enabled";
            this.viewportEnabledCheckBox.UseVisualStyleBackColor = true;
            this.viewportEnabledCheckBox.CheckedChanged += new System.EventHandler(this.viewportEnabledCheckBox_CheckedChanged);
            // 
            // scaleNumericUpDown
            // 
            this.scaleNumericUpDown.Location = new System.Drawing.Point(47, 97);
            this.scaleNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.scaleNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scaleNumericUpDown.Name = "scaleNumericUpDown";
            this.scaleNumericUpDown.Size = new System.Drawing.Size(53, 20);
            this.scaleNumericUpDown.TabIndex = 8;
            this.scaleNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Scale";
            // 
            // landscapeCheckBox
            // 
            this.landscapeCheckBox.AutoSize = true;
            this.landscapeCheckBox.Location = new System.Drawing.Point(128, 97);
            this.landscapeCheckBox.Name = "landscapeCheckBox";
            this.landscapeCheckBox.Size = new System.Drawing.Size(79, 17);
            this.landscapeCheckBox.TabIndex = 6;
            this.landscapeCheckBox.Text = "Landscape";
            this.landscapeCheckBox.UseVisualStyleBackColor = true;
            // 
            // touchCheckBox
            // 
            this.touchCheckBox.AutoSize = true;
            this.touchCheckBox.Location = new System.Drawing.Point(128, 30);
            this.touchCheckBox.Name = "touchCheckBox";
            this.touchCheckBox.Size = new System.Drawing.Size(57, 17);
            this.touchCheckBox.TabIndex = 5;
            this.touchCheckBox.Text = "Touch";
            this.touchCheckBox.UseVisualStyleBackColor = true;
            // 
            // mobileCheckBox
            // 
            this.mobileCheckBox.AutoSize = true;
            this.mobileCheckBox.Location = new System.Drawing.Point(128, 64);
            this.mobileCheckBox.Name = "mobileCheckBox";
            this.mobileCheckBox.Size = new System.Drawing.Size(57, 17);
            this.mobileCheckBox.TabIndex = 4;
            this.mobileCheckBox.Text = "Mobile";
            this.mobileCheckBox.UseVisualStyleBackColor = true;
            // 
            // heightNumericUpDown
            // 
            this.heightNumericUpDown.Location = new System.Drawing.Point(47, 64);
            this.heightNumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.heightNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightNumericUpDown.Name = "heightNumericUpDown";
            this.heightNumericUpDown.Size = new System.Drawing.Size(53, 20);
            this.heightNumericUpDown.TabIndex = 3;
            this.heightNumericUpDown.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Width";
            // 
            // widthNumericUpDown
            // 
            this.widthNumericUpDown.Location = new System.Drawing.Point(47, 30);
            this.widthNumericUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.widthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthNumericUpDown.Name = "widthNumericUpDown";
            this.widthNumericUpDown.Size = new System.Drawing.Size(53, 20);
            this.widthNumericUpDown.TabIndex = 0;
            this.widthNumericUpDown.Value = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Browser";
            // 
            // connectionTypeComboBox
            // 
            this.connectionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionTypeComboBox.FormattingEnabled = true;
            this.connectionTypeComboBox.Items.AddRange(new object[] {
            "Launch",
            "Connect"});
            this.connectionTypeComboBox.Location = new System.Drawing.Point(98, 181);
            this.connectionTypeComboBox.Name = "connectionTypeComboBox";
            this.connectionTypeComboBox.Size = new System.Drawing.Size(84, 21);
            this.connectionTypeComboBox.TabIndex = 2;
            this.connectionTypeComboBox.SelectedValueChanged += new System.EventHandler(this.connectionTypeComboBox_SelectedValueChanged);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(99, 219);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(29, 13);
            this.pathLabel.TabIndex = 3;
            this.pathLabel.Text = "Path";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(3, 235);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(228, 20);
            this.pathTextBox.TabIndex = 4;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(76, 261);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // hostLabel
            // 
            this.hostLabel.AutoSize = true;
            this.hostLabel.Location = new System.Drawing.Point(5, 219);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(29, 13);
            this.hostLabel.TabIndex = 6;
            this.hostLabel.Text = "Host";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(156, 219);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 7;
            this.portLabel.Text = "Port";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Location = new System.Drawing.Point(35, 216);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(100, 20);
            this.hostTextBox.TabIndex = 8;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(188, 216);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(41, 20);
            this.portTextBox.TabIndex = 9;
            // 
            // devtoolsCheckBox
            // 
            this.devtoolsCheckBox.AutoSize = true;
            this.devtoolsCheckBox.Location = new System.Drawing.Point(142, 156);
            this.devtoolsCheckBox.Name = "devtoolsCheckBox";
            this.devtoolsCheckBox.Size = new System.Drawing.Size(68, 17);
            this.devtoolsCheckBox.TabIndex = 10;
            this.devtoolsCheckBox.Text = "Devtools";
            this.devtoolsCheckBox.UseVisualStyleBackColor = true;
            // 
            // slowMoLabel
            // 
            this.slowMoLabel.AutoSize = true;
            this.slowMoLabel.Location = new System.Drawing.Point(9, 144);
            this.slowMoLabel.Name = "slowMoLabel";
            this.slowMoLabel.Size = new System.Drawing.Size(45, 13);
            this.slowMoLabel.TabIndex = 12;
            this.slowMoLabel.Text = "SlowMo";
            // 
            // slowMoNumericUpDown
            // 
            this.slowMoNumericUpDown.Location = new System.Drawing.Point(60, 141);
            this.slowMoNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.slowMoNumericUpDown.Name = "slowMoNumericUpDown";
            this.slowMoNumericUpDown.Size = new System.Drawing.Size(57, 20);
            this.slowMoNumericUpDown.TabIndex = 13;
            this.slowMoNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // headlessCheckBox
            // 
            this.headlessCheckBox.AutoSize = true;
            this.headlessCheckBox.Location = new System.Drawing.Point(142, 133);
            this.headlessCheckBox.Name = "headlessCheckBox";
            this.headlessCheckBox.Size = new System.Drawing.Size(70, 17);
            this.headlessCheckBox.TabIndex = 14;
            this.headlessCheckBox.Text = "Headless";
            this.headlessCheckBox.UseVisualStyleBackColor = true;
            // 
            // PuppeteerOptionsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.headlessCheckBox);
            this.Controls.Add(this.slowMoNumericUpDown);
            this.Controls.Add(this.slowMoLabel);
            this.Controls.Add(this.devtoolsCheckBox);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.hostTextBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.hostLabel);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.connectionTypeComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.viewportGroupBox);
            this.Name = "PuppeteerOptionsUserControl";
            this.Size = new System.Drawing.Size(236, 290);
            this.Load += new System.EventHandler(this.PuppeteerOptionsUserControl_Load);
            this.viewportGroupBox.ResumeLayout(false);
            this.viewportGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slowMoNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox viewportGroupBox;
        private System.Windows.Forms.CheckBox landscapeCheckBox;
        private System.Windows.Forms.CheckBox touchCheckBox;
        private System.Windows.Forms.CheckBox mobileCheckBox;
        private System.Windows.Forms.NumericUpDown heightNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown widthNumericUpDown;
        private System.Windows.Forms.NumericUpDown scaleNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox connectionTypeComboBox;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label hostLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.CheckBox devtoolsCheckBox;
        private System.Windows.Forms.Label slowMoLabel;
        private System.Windows.Forms.NumericUpDown slowMoNumericUpDown;
        private System.Windows.Forms.CheckBox viewportEnabledCheckBox;
        private System.Windows.Forms.CheckBox headlessCheckBox;
    }
}
