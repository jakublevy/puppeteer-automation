namespace Frontend.Forms
{
    partial class FilterForm
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
            this.typeGroupBox = new System.Windows.Forms.GroupBox();
            this.viewportEventsEnabled = new System.Windows.Forms.CheckBox();
            this.viewportEventsGroupBox = new System.Windows.Forms.GroupBox();
            this.mouseoverCheckBox = new System.Windows.Forms.CheckBox();
            this.copyCheckBox = new System.Windows.Forms.CheckBox();
            this.submitCheckBox = new System.Windows.Forms.CheckBox();
            this.inputChangeBox = new System.Windows.Forms.CheckBox();
            this.dblclickCheckBox = new System.Windows.Forms.CheckBox();
            this.pasteCheckBox = new System.Windows.Forms.CheckBox();
            this.scrollCheckBox = new System.Windows.Forms.CheckBox();
            this.selectChangeBox = new System.Windows.Forms.CheckBox();
            this.changeCheckBox = new System.Windows.Forms.CheckBox();
            this.clickCheckBox = new System.Windows.Forms.CheckBox();
            this.puppeteerEventsEnabled = new System.Windows.Forms.CheckBox();
            this.puppeteerEventsGroupBox = new System.Windows.Forms.GroupBox();
            this.pageUrlChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.pageOpenedCheckBox = new System.Windows.Forms.CheckBox();
            this.pageClosedCheckBox = new System.Windows.Forms.CheckBox();
            this.pageSwitchedCheckBox = new System.Windows.Forms.CheckBox();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.uncheckedCheckBox = new System.Windows.Forms.CheckBox();
            this.checkedCheckBox = new System.Windows.Forms.CheckBox();
            this.disabledCheckBox = new System.Windows.Forms.CheckBox();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.targetGroupBox = new System.Windows.Forms.GroupBox();
            this.selectorCheckBox = new System.Windows.Forms.CheckBox();
            this.locatorCheckBox = new System.Windows.Forms.CheckBox();
            this.statusEnabled = new System.Windows.Forms.CheckBox();
            this.targetEnabled = new System.Windows.Forms.CheckBox();
            this.typeEnabled = new System.Windows.Forms.CheckBox();
            this.typeGroupBox.SuspendLayout();
            this.viewportEventsGroupBox.SuspendLayout();
            this.puppeteerEventsGroupBox.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            this.targetGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeGroupBox
            // 
            this.typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeGroupBox.Controls.Add(this.viewportEventsEnabled);
            this.typeGroupBox.Controls.Add(this.viewportEventsGroupBox);
            this.typeGroupBox.Controls.Add(this.puppeteerEventsEnabled);
            this.typeGroupBox.Controls.Add(this.puppeteerEventsGroupBox);
            this.typeGroupBox.Enabled = false;
            this.typeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.typeGroupBox.Name = "typeGroupBox";
            this.typeGroupBox.Size = new System.Drawing.Size(256, 249);
            this.typeGroupBox.TabIndex = 0;
            this.typeGroupBox.TabStop = false;
            this.typeGroupBox.Text = "By Type";
            // 
            // viewportEventsEnabled
            // 
            this.viewportEventsEnabled.AutoSize = true;
            this.viewportEventsEnabled.Location = new System.Drawing.Point(98, 90);
            this.viewportEventsEnabled.Name = "viewportEventsEnabled";
            this.viewportEventsEnabled.Size = new System.Drawing.Size(65, 17);
            this.viewportEventsEnabled.TabIndex = 0;
            this.viewportEventsEnabled.Text = "Enabled";
            this.viewportEventsEnabled.UseVisualStyleBackColor = true;
            this.viewportEventsEnabled.CheckedChanged += new System.EventHandler(this.viewportEventsEnabled_CheckedChanged);
            // 
            // viewportEventsGroupBox
            // 
            this.viewportEventsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewportEventsGroupBox.Controls.Add(this.mouseoverCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.copyCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.submitCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.inputChangeBox);
            this.viewportEventsGroupBox.Controls.Add(this.dblclickCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.pasteCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.scrollCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.selectChangeBox);
            this.viewportEventsGroupBox.Controls.Add(this.changeCheckBox);
            this.viewportEventsGroupBox.Controls.Add(this.clickCheckBox);
            this.viewportEventsGroupBox.Enabled = false;
            this.viewportEventsGroupBox.Location = new System.Drawing.Point(6, 93);
            this.viewportEventsGroupBox.Name = "viewportEventsGroupBox";
            this.viewportEventsGroupBox.Size = new System.Drawing.Size(244, 142);
            this.viewportEventsGroupBox.TabIndex = 1;
            this.viewportEventsGroupBox.TabStop = false;
            this.viewportEventsGroupBox.Text = "Viewport Events";
            // 
            // mouseoverCheckBox
            // 
            this.mouseoverCheckBox.AutoSize = true;
            this.mouseoverCheckBox.Checked = true;
            this.mouseoverCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mouseoverCheckBox.Location = new System.Drawing.Point(136, 111);
            this.mouseoverCheckBox.Name = "mouseoverCheckBox";
            this.mouseoverCheckBox.Size = new System.Drawing.Size(79, 17);
            this.mouseoverCheckBox.TabIndex = 9;
            this.mouseoverCheckBox.Text = "Mouseover";
            this.mouseoverCheckBox.UseVisualStyleBackColor = true;
            this.mouseoverCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // copyCheckBox
            // 
            this.copyCheckBox.AutoSize = true;
            this.copyCheckBox.Checked = true;
            this.copyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyCheckBox.Location = new System.Drawing.Point(136, 88);
            this.copyCheckBox.Name = "copyCheckBox";
            this.copyCheckBox.Size = new System.Drawing.Size(50, 17);
            this.copyCheckBox.TabIndex = 8;
            this.copyCheckBox.Text = "Copy";
            this.copyCheckBox.UseVisualStyleBackColor = true;
            this.copyCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // submitCheckBox
            // 
            this.submitCheckBox.AutoSize = true;
            this.submitCheckBox.Checked = true;
            this.submitCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.submitCheckBox.Location = new System.Drawing.Point(136, 65);
            this.submitCheckBox.Name = "submitCheckBox";
            this.submitCheckBox.Size = new System.Drawing.Size(58, 17);
            this.submitCheckBox.TabIndex = 7;
            this.submitCheckBox.Text = "Submit";
            this.submitCheckBox.UseVisualStyleBackColor = true;
            this.submitCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // inputChangeBox
            // 
            this.inputChangeBox.AutoSize = true;
            this.inputChangeBox.Checked = true;
            this.inputChangeBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inputChangeBox.Location = new System.Drawing.Point(136, 42);
            this.inputChangeBox.Name = "inputChangeBox";
            this.inputChangeBox.Size = new System.Drawing.Size(50, 17);
            this.inputChangeBox.TabIndex = 6;
            this.inputChangeBox.Text = "Input";
            this.inputChangeBox.UseVisualStyleBackColor = true;
            this.inputChangeBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // dblclickCheckBox
            // 
            this.dblclickCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dblclickCheckBox.AutoSize = true;
            this.dblclickCheckBox.Checked = true;
            this.dblclickCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dblclickCheckBox.Location = new System.Drawing.Point(136, 19);
            this.dblclickCheckBox.Name = "dblclickCheckBox";
            this.dblclickCheckBox.Size = new System.Drawing.Size(64, 17);
            this.dblclickCheckBox.TabIndex = 5;
            this.dblclickCheckBox.Text = "Dblclick";
            this.dblclickCheckBox.UseVisualStyleBackColor = true;
            this.dblclickCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // pasteCheckBox
            // 
            this.pasteCheckBox.AutoSize = true;
            this.pasteCheckBox.Checked = true;
            this.pasteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pasteCheckBox.Location = new System.Drawing.Point(27, 111);
            this.pasteCheckBox.Name = "pasteCheckBox";
            this.pasteCheckBox.Size = new System.Drawing.Size(53, 17);
            this.pasteCheckBox.TabIndex = 4;
            this.pasteCheckBox.Text = "Paste";
            this.pasteCheckBox.UseVisualStyleBackColor = true;
            this.pasteCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // scrollCheckBox
            // 
            this.scrollCheckBox.AutoSize = true;
            this.scrollCheckBox.Checked = true;
            this.scrollCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scrollCheckBox.Location = new System.Drawing.Point(27, 88);
            this.scrollCheckBox.Name = "scrollCheckBox";
            this.scrollCheckBox.Size = new System.Drawing.Size(52, 17);
            this.scrollCheckBox.TabIndex = 3;
            this.scrollCheckBox.Text = "Scroll";
            this.scrollCheckBox.UseVisualStyleBackColor = true;
            this.scrollCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // selectChangeBox
            // 
            this.selectChangeBox.AutoSize = true;
            this.selectChangeBox.Checked = true;
            this.selectChangeBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectChangeBox.Location = new System.Drawing.Point(27, 65);
            this.selectChangeBox.Name = "selectChangeBox";
            this.selectChangeBox.Size = new System.Drawing.Size(56, 17);
            this.selectChangeBox.TabIndex = 2;
            this.selectChangeBox.Text = "Select";
            this.selectChangeBox.UseVisualStyleBackColor = true;
            this.selectChangeBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // changeCheckBox
            // 
            this.changeCheckBox.AutoSize = true;
            this.changeCheckBox.Checked = true;
            this.changeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.changeCheckBox.Location = new System.Drawing.Point(27, 42);
            this.changeCheckBox.Name = "changeCheckBox";
            this.changeCheckBox.Size = new System.Drawing.Size(63, 17);
            this.changeCheckBox.TabIndex = 1;
            this.changeCheckBox.Text = "Change";
            this.changeCheckBox.UseVisualStyleBackColor = true;
            this.changeCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // clickCheckBox
            // 
            this.clickCheckBox.AutoSize = true;
            this.clickCheckBox.Checked = true;
            this.clickCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.clickCheckBox.Location = new System.Drawing.Point(27, 19);
            this.clickCheckBox.Name = "clickCheckBox";
            this.clickCheckBox.Size = new System.Drawing.Size(49, 17);
            this.clickCheckBox.TabIndex = 0;
            this.clickCheckBox.Text = "Click";
            this.clickCheckBox.UseVisualStyleBackColor = true;
            this.clickCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // puppeteerEventsEnabled
            // 
            this.puppeteerEventsEnabled.AutoSize = true;
            this.puppeteerEventsEnabled.Location = new System.Drawing.Point(117, 17);
            this.puppeteerEventsEnabled.Name = "puppeteerEventsEnabled";
            this.puppeteerEventsEnabled.Size = new System.Drawing.Size(65, 17);
            this.puppeteerEventsEnabled.TabIndex = 6;
            this.puppeteerEventsEnabled.Text = "Enabled";
            this.puppeteerEventsEnabled.UseVisualStyleBackColor = true;
            this.puppeteerEventsEnabled.CheckedChanged += new System.EventHandler(this.puppeteerEventsEnabled_CheckedChanged);
            // 
            // puppeteerEventsGroupBox
            // 
            this.puppeteerEventsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.puppeteerEventsGroupBox.Controls.Add(this.pageUrlChangedCheckBox);
            this.puppeteerEventsGroupBox.Controls.Add(this.pageOpenedCheckBox);
            this.puppeteerEventsGroupBox.Controls.Add(this.pageClosedCheckBox);
            this.puppeteerEventsGroupBox.Controls.Add(this.pageSwitchedCheckBox);
            this.puppeteerEventsGroupBox.Enabled = false;
            this.puppeteerEventsGroupBox.Location = new System.Drawing.Point(6, 19);
            this.puppeteerEventsGroupBox.Name = "puppeteerEventsGroupBox";
            this.puppeteerEventsGroupBox.Size = new System.Drawing.Size(244, 68);
            this.puppeteerEventsGroupBox.TabIndex = 0;
            this.puppeteerEventsGroupBox.TabStop = false;
            this.puppeteerEventsGroupBox.Text = "Browser App Events";
            // 
            // pageUrlChangedCheckBox
            // 
            this.pageUrlChangedCheckBox.AutoSize = true;
            this.pageUrlChangedCheckBox.Checked = true;
            this.pageUrlChangedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pageUrlChangedCheckBox.Location = new System.Drawing.Point(136, 43);
            this.pageUrlChangedCheckBox.Name = "pageUrlChangedCheckBox";
            this.pageUrlChangedCheckBox.Size = new System.Drawing.Size(106, 17);
            this.pageUrlChangedCheckBox.TabIndex = 3;
            this.pageUrlChangedCheckBox.Text = "pageUrlChanged";
            this.pageUrlChangedCheckBox.UseVisualStyleBackColor = true;
            this.pageUrlChangedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // pageOpenedCheckBox
            // 
            this.pageOpenedCheckBox.AutoSize = true;
            this.pageOpenedCheckBox.Checked = true;
            this.pageOpenedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pageOpenedCheckBox.Location = new System.Drawing.Point(27, 43);
            this.pageOpenedCheckBox.Name = "pageOpenedCheckBox";
            this.pageOpenedCheckBox.Size = new System.Drawing.Size(88, 17);
            this.pageOpenedCheckBox.TabIndex = 1;
            this.pageOpenedCheckBox.Text = "pageOpened";
            this.pageOpenedCheckBox.UseVisualStyleBackColor = true;
            this.pageOpenedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // pageClosedCheckBox
            // 
            this.pageClosedCheckBox.AutoSize = true;
            this.pageClosedCheckBox.Checked = true;
            this.pageClosedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pageClosedCheckBox.Location = new System.Drawing.Point(136, 19);
            this.pageClosedCheckBox.Name = "pageClosedCheckBox";
            this.pageClosedCheckBox.Size = new System.Drawing.Size(82, 17);
            this.pageClosedCheckBox.TabIndex = 2;
            this.pageClosedCheckBox.Text = "pageClosed";
            this.pageClosedCheckBox.UseVisualStyleBackColor = true;
            this.pageClosedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // pageSwitchedCheckBox
            // 
            this.pageSwitchedCheckBox.AutoSize = true;
            this.pageSwitchedCheckBox.Checked = true;
            this.pageSwitchedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pageSwitchedCheckBox.Location = new System.Drawing.Point(27, 19);
            this.pageSwitchedCheckBox.Name = "pageSwitchedCheckBox";
            this.pageSwitchedCheckBox.Size = new System.Drawing.Size(94, 17);
            this.pageSwitchedCheckBox.TabIndex = 0;
            this.pageSwitchedCheckBox.Text = "pageSwitched";
            this.pageSwitchedCheckBox.UseVisualStyleBackColor = true;
            this.pageSwitchedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusGroupBox.Controls.Add(this.uncheckedCheckBox);
            this.statusGroupBox.Controls.Add(this.checkedCheckBox);
            this.statusGroupBox.Controls.Add(this.disabledCheckBox);
            this.statusGroupBox.Controls.Add(this.enabledCheckBox);
            this.statusGroupBox.Enabled = false;
            this.statusGroupBox.Location = new System.Drawing.Point(12, 320);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(256, 68);
            this.statusGroupBox.TabIndex = 1;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "By Status";
            // 
            // uncheckedCheckBox
            // 
            this.uncheckedCheckBox.AutoSize = true;
            this.uncheckedCheckBox.Checked = true;
            this.uncheckedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uncheckedCheckBox.Location = new System.Drawing.Point(140, 38);
            this.uncheckedCheckBox.Name = "uncheckedCheckBox";
            this.uncheckedCheckBox.Size = new System.Drawing.Size(88, 17);
            this.uncheckedCheckBox.TabIndex = 3;
            this.uncheckedCheckBox.Text = "Not Selected";
            this.uncheckedCheckBox.UseVisualStyleBackColor = true;
            this.uncheckedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // checkedCheckBox
            // 
            this.checkedCheckBox.AutoSize = true;
            this.checkedCheckBox.Checked = true;
            this.checkedCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkedCheckBox.Location = new System.Drawing.Point(140, 15);
            this.checkedCheckBox.Name = "checkedCheckBox";
            this.checkedCheckBox.Size = new System.Drawing.Size(68, 17);
            this.checkedCheckBox.TabIndex = 2;
            this.checkedCheckBox.Text = "Selected";
            this.checkedCheckBox.UseVisualStyleBackColor = true;
            this.checkedCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // disabledCheckBox
            // 
            this.disabledCheckBox.AutoSize = true;
            this.disabledCheckBox.Checked = true;
            this.disabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disabledCheckBox.Location = new System.Drawing.Point(33, 38);
            this.disabledCheckBox.Name = "disabledCheckBox";
            this.disabledCheckBox.Size = new System.Drawing.Size(67, 17);
            this.disabledCheckBox.TabIndex = 1;
            this.disabledCheckBox.Text = "Disabled";
            this.disabledCheckBox.UseVisualStyleBackColor = true;
            this.disabledCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Checked = true;
            this.enabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enabledCheckBox.Location = new System.Drawing.Point(33, 15);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(65, 17);
            this.enabledCheckBox.TabIndex = 0;
            this.enabledCheckBox.Text = "Enabled";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetGroupBox.Controls.Add(this.selectorCheckBox);
            this.targetGroupBox.Controls.Add(this.locatorCheckBox);
            this.targetGroupBox.Enabled = false;
            this.targetGroupBox.Location = new System.Drawing.Point(12, 267);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.Size = new System.Drawing.Size(256, 47);
            this.targetGroupBox.TabIndex = 2;
            this.targetGroupBox.TabStop = false;
            this.targetGroupBox.Text = "By Target";
            // 
            // selectorCheckBox
            // 
            this.selectorCheckBox.AutoSize = true;
            this.selectorCheckBox.Checked = true;
            this.selectorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectorCheckBox.Location = new System.Drawing.Point(142, 19);
            this.selectorCheckBox.Name = "selectorCheckBox";
            this.selectorCheckBox.Size = new System.Drawing.Size(65, 17);
            this.selectorCheckBox.TabIndex = 1;
            this.selectorCheckBox.Text = "Selector";
            this.selectorCheckBox.UseVisualStyleBackColor = true;
            this.selectorCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // locatorCheckBox
            // 
            this.locatorCheckBox.AutoSize = true;
            this.locatorCheckBox.Checked = true;
            this.locatorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.locatorCheckBox.Location = new System.Drawing.Point(33, 19);
            this.locatorCheckBox.Name = "locatorCheckBox";
            this.locatorCheckBox.Size = new System.Drawing.Size(62, 17);
            this.locatorCheckBox.TabIndex = 0;
            this.locatorCheckBox.Text = "Locator";
            this.locatorCheckBox.UseVisualStyleBackColor = true;
            this.locatorCheckBox.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // statusEnabled
            // 
            this.statusEnabled.AutoSize = true;
            this.statusEnabled.Location = new System.Drawing.Point(72, 318);
            this.statusEnabled.Name = "statusEnabled";
            this.statusEnabled.Size = new System.Drawing.Size(65, 17);
            this.statusEnabled.TabIndex = 2;
            this.statusEnabled.Text = "Enabled";
            this.statusEnabled.UseVisualStyleBackColor = true;
            this.statusEnabled.CheckedChanged += new System.EventHandler(this.statusEnabled_CheckedChanged);
            // 
            // targetEnabled
            // 
            this.targetEnabled.AutoSize = true;
            this.targetEnabled.Location = new System.Drawing.Point(72, 266);
            this.targetEnabled.Name = "targetEnabled";
            this.targetEnabled.Size = new System.Drawing.Size(65, 17);
            this.targetEnabled.TabIndex = 1;
            this.targetEnabled.Text = "Enabled";
            this.targetEnabled.UseVisualStyleBackColor = true;
            this.targetEnabled.CheckedChanged += new System.EventHandler(this.targetEnabled_CheckedChanged);
            // 
            // typeEnabled
            // 
            this.typeEnabled.AutoSize = true;
            this.typeEnabled.Location = new System.Drawing.Point(66, 11);
            this.typeEnabled.Name = "typeEnabled";
            this.typeEnabled.Size = new System.Drawing.Size(65, 17);
            this.typeEnabled.TabIndex = 0;
            this.typeEnabled.Text = "Enabled";
            this.typeEnabled.UseVisualStyleBackColor = true;
            this.typeEnabled.CheckedChanged += new System.EventHandler(this.typeEnabled_CheckedChanged);
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 393);
            this.Controls.Add(this.typeEnabled);
            this.Controls.Add(this.targetEnabled);
            this.Controls.Add(this.statusEnabled);
            this.Controls.Add(this.targetGroupBox);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.typeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FilterForm";
            this.Text = "Filter";
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.viewportEventsGroupBox.ResumeLayout(false);
            this.viewportEventsGroupBox.PerformLayout();
            this.puppeteerEventsGroupBox.ResumeLayout(false);
            this.puppeteerEventsGroupBox.PerformLayout();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.GroupBox puppeteerEventsGroupBox;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.CheckBox uncheckedCheckBox;
        private System.Windows.Forms.CheckBox checkedCheckBox;
        private System.Windows.Forms.CheckBox disabledCheckBox;
        private System.Windows.Forms.CheckBox enabledCheckBox;
        private System.Windows.Forms.GroupBox targetGroupBox;
        private System.Windows.Forms.CheckBox selectorCheckBox;
        private System.Windows.Forms.CheckBox locatorCheckBox;
        private System.Windows.Forms.GroupBox viewportEventsGroupBox;
        private System.Windows.Forms.CheckBox mouseoverCheckBox;
        private System.Windows.Forms.CheckBox copyCheckBox;
        private System.Windows.Forms.CheckBox submitCheckBox;
        private System.Windows.Forms.CheckBox inputChangeBox;
        private System.Windows.Forms.CheckBox dblclickCheckBox;
        private System.Windows.Forms.CheckBox pasteCheckBox;
        private System.Windows.Forms.CheckBox scrollCheckBox;
        private System.Windows.Forms.CheckBox selectChangeBox;
        private System.Windows.Forms.CheckBox changeCheckBox;
        private System.Windows.Forms.CheckBox clickCheckBox;
        private System.Windows.Forms.CheckBox pageUrlChangedCheckBox;
        private System.Windows.Forms.CheckBox pageOpenedCheckBox;
        private System.Windows.Forms.CheckBox pageClosedCheckBox;
        private System.Windows.Forms.CheckBox pageSwitchedCheckBox;
        private System.Windows.Forms.CheckBox statusEnabled;
        private System.Windows.Forms.CheckBox targetEnabled;
        private System.Windows.Forms.CheckBox viewportEventsEnabled;
        private System.Windows.Forms.CheckBox puppeteerEventsEnabled;
        private System.Windows.Forms.CheckBox typeEnabled;
    }
}