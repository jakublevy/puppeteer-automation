namespace Frontend.UserControls
{
    partial class ActionUserControl
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
            this.jsonEditButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.locatorLabel = new System.Windows.Forms.Label();
            this.locatorsComboBox = new System.Windows.Forms.ComboBox();
            this.selectorLabel = new System.Windows.Forms.Label();
            this.selectorTextBox = new System.Windows.Forms.TextBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.selectCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // jsonEditButton
            // 
            this.jsonEditButton.Location = new System.Drawing.Point(583, 3);
            this.jsonEditButton.Name = "jsonEditButton";
            this.jsonEditButton.Size = new System.Drawing.Size(75, 23);
            this.jsonEditButton.TabIndex = 0;
            this.jsonEditButton.Text = "JSON Edit";
            this.jsonEditButton.UseVisualStyleBackColor = true;
            this.jsonEditButton.Click += new System.EventHandler(this.jsonEditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(583, 26);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(664, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(27, 23);
            this.upButton.TabIndex = 2;
            this.upButton.Text = "↑";
            this.upButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(664, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "↓";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(31, 20);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(116, 21);
            this.typeComboBox.TabIndex = 4;
            this.typeComboBox.SelectedValueChanged += new System.EventHandler(this.typeComboBox_SelectedValueChanged);
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(29, 4);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(31, 13);
            this.typeLabel.TabIndex = 5;
            this.typeLabel.Text = "Type";
            // 
            // locatorLabel
            // 
            this.locatorLabel.AutoSize = true;
            this.locatorLabel.Location = new System.Drawing.Point(154, 3);
            this.locatorLabel.Name = "locatorLabel";
            this.locatorLabel.Size = new System.Drawing.Size(48, 13);
            this.locatorLabel.TabIndex = 6;
            this.locatorLabel.Text = "Locators";
            // 
            // locatorsComboBox
            // 
            this.locatorsComboBox.FormattingEnabled = true;
            this.locatorsComboBox.Location = new System.Drawing.Point(157, 20);
            this.locatorsComboBox.Name = "locatorsComboBox";
            this.locatorsComboBox.Size = new System.Drawing.Size(121, 21);
            this.locatorsComboBox.TabIndex = 7;
            // 
            // selectorLabel
            // 
            this.selectorLabel.AutoSize = true;
            this.selectorLabel.Location = new System.Drawing.Point(287, 4);
            this.selectorLabel.Name = "selectorLabel";
            this.selectorLabel.Size = new System.Drawing.Size(46, 13);
            this.selectorLabel.TabIndex = 8;
            this.selectorLabel.Text = "Selector";
            // 
            // selectorTextBox
            // 
            this.selectorTextBox.Location = new System.Drawing.Point(290, 20);
            this.selectorTextBox.Name = "selectorTextBox";
            this.selectorTextBox.Size = new System.Drawing.Size(100, 20);
            this.selectorTextBox.TabIndex = 9;
            this.selectorTextBox.TextChanged += new System.EventHandler(this.selectorTextBox_TextChanged);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(400, 4);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(34, 13);
            this.valueLabel.TabIndex = 10;
            this.valueLabel.Text = "Value";
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(403, 20);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(100, 20);
            this.valueTextBox.TabIndex = 11;
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Checked = true;
            this.enabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enabledCheckBox.Location = new System.Drawing.Point(512, 20);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(65, 17);
            this.enabledCheckBox.TabIndex = 12;
            this.enabledCheckBox.Text = "Enabled";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // selectCheckBox
            // 
            this.selectCheckBox.AutoSize = true;
            this.selectCheckBox.Location = new System.Drawing.Point(7, 20);
            this.selectCheckBox.Name = "selectCheckBox";
            this.selectCheckBox.Size = new System.Drawing.Size(15, 14);
            this.selectCheckBox.TabIndex = 13;
            this.selectCheckBox.UseVisualStyleBackColor = true;
            // 
            // ActionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectCheckBox);
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.selectorTextBox);
            this.Controls.Add(this.selectorLabel);
            this.Controls.Add(this.locatorsComboBox);
            this.Controls.Add(this.locatorLabel);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.jsonEditButton);
            this.Name = "ActionUserControl";
            this.Size = new System.Drawing.Size(701, 53);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button jsonEditButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label locatorLabel;
        private System.Windows.Forms.ComboBox locatorsComboBox;
        private System.Windows.Forms.Label selectorLabel;
        private System.Windows.Forms.TextBox selectorTextBox;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.CheckBox selectCheckBox;
    }
}
