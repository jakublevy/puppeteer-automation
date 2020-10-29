using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Frontend.UserControls;

namespace Frontend.Forms
{
    public partial class FilterForm : Form
    {
        private EditUserControl editUserControl;
        public FilterForm(EditUserControl launcher)
        {
            InitializeComponent();
            editUserControl = launcher;
        }

        public void SetFilter(Filter f)
        {
            if (f.EventTypes.Count > 0)
            {
                typeEnabled.Checked = true;
                foreach (CheckBox c in puppeteerEventsGroupBox.Controls)
                {
                    if (f.EventTypes.Contains(c.Text))
                    {
                        c.Checked = false;
                        puppeteerEventsEnabled.Checked = true;
                    }
                }

                foreach (CheckBox c in viewportEventsGroupBox.Controls)
                {
                    if (f.EventTypes.Contains(c.Text))
                    {
                        c.Checked = false;
                        viewportEventsEnabled.Checked = true;
                    }
                }
            }

            if (f.Targets.Count > 0)
            {
                targetEnabled.Checked = true;
                foreach (CheckBox c in targetGroupBox.Controls)
                {
                    if (f.Targets.Contains(c.Text))
                        c.Checked = false;
                }
            }

            if (f.Status.Count > 0)
            {
                statusEnabled.Enabled = true;
                foreach (CheckBox c in statusGroupBox.Controls)
                {
                    if (f.Status.Contains(c.Text))
                    {
                        c.Checked = false;
                    }
                }
            }
        }

        public Filter ExportFilter()
        {
            Filter f = new Filter();

            if (typeEnabled.Checked)
            {
                if (puppeteerEventsEnabled.Checked)
                    f.EventTypes.AddRange(EventsToHide(puppeteerEventsGroupBox));
                

                if (viewportEventsEnabled.Checked)
                    f.EventTypes.AddRange(EventsToHide(viewportEventsGroupBox));
                
            }

            if (targetEnabled.Checked)
                f.Targets = EventsToHide(targetGroupBox);


            if (statusEnabled.Checked)
                f.Status = EventsToHide(statusGroupBox);

            return f;
        }

        private List<string> EventsToHide(GroupBox parent)
        {
            List<string> output = new List<string>();
            foreach (CheckBox c in parent.Controls)
            {
                if (!c.Checked)
                    output.Add(c.Text);
                
            }

            return output;
        }

        private void typeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            typeGroupBox.Enabled = typeEnabled.Checked;
            editUserControl.FilterChanged(ExportFilter());
        }

        private void puppeteerEventsEnabled_CheckedChanged(object sender, EventArgs e)
        {
            puppeteerEventsGroupBox.Enabled = puppeteerEventsEnabled.Checked;
            editUserControl.FilterChanged(ExportFilter());
        }

        private void viewportEventsEnabled_CheckedChanged(object sender, EventArgs e)
        {
            viewportEventsGroupBox.Enabled = viewportEventsEnabled.Checked;
            editUserControl.FilterChanged(ExportFilter());
        }

        private void targetEnabled_CheckedChanged(object sender, EventArgs e)
        {
            targetGroupBox.Enabled = targetEnabled.Checked;
            editUserControl.FilterChanged(ExportFilter());
        }

        private void statusEnabled_CheckedChanged(object sender, EventArgs e)
        {
            statusGroupBox.Enabled = statusEnabled.Checked;
            editUserControl.FilterChanged(ExportFilter());
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            editUserControl.FilterChanged(ExportFilter());
        }
    }
}
