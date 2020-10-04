using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend.Forms;
using Newtonsoft.Json;

namespace Frontend.UserControls
{
    public partial class ActionUserControl : UserControl
    {
        private dynamic action;
        private UiConfig uiConfig;
        private JsonEditor je = null;

        public bool Selected => selectCheckBox.Checked;
        public bool EnabledForOutput => enabledCheckBox.Checked;

        public ActionUserControl()
        {
            InitializeComponent();
        }

        public void BindRecording(Recording r)
        {
            action = r.Action;
            uiConfig = r.UiConfig;
            BindActionToUi();
            ApplyUi();
        }

        private void ApplyUi()
        {
            enabledCheckBox.Checked = uiConfig.Enabled;
            selectCheckBox.Checked = uiConfig.Selected;
            if(locatorsComboBox.Items.Count > 0)
                locatorsComboBox.SelectedIndex = uiConfig.SelectedLocatorIndex;

            if (uiConfig.Target == Target.Selector)
                selectorRadioButton.Checked = true;
            else
                locatorRadioButton.Checked = true;
        }

        public void BindActionToUi()
        {
            typeComboBox.Items.Clear();

            int oldSelectedLocatorIndex = locatorsComboBox.SelectedIndex;

            locatorsComboBox.Items.Clear();

            if (action.type == "click" || action.type == "dblclick")
            {
                typeComboBox.Items.Add("click");
                typeComboBox.Items.Add("dblclick");
            }
            else
                typeComboBox.Items.Add(action.type);

            typeComboBox.SelectedIndex = action.type == "dblclick" ? 1 : 0;

            if (action.locators != null && action.locators.Count > 0)
            {
                foreach (var locator in action.locators)
                    locatorsComboBox.Items.Add(locator.locator);
                

                if (oldSelectedLocatorIndex != -1)
                    locatorsComboBox.SelectedIndex = oldSelectedLocatorIndex;
            }
            selectorTextBox.Text = action.selector;
            valueTextBox.Text = action.value;

        }

        public Recording ExportRecordingForSave()
        {
            return new Recording {Action = action, UiConfig = uiConfig};
        }

        public dynamic ExportActionForOutput()
        {
            dynamic output = null;
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    output = ExportActionForOutputImpl();
                }));
            }
            else
            {
                output = ExportActionForOutputImpl();
            }

            return output;
        }

        public dynamic ExportActionForOutputImpl()
        {
            if (action.locators != null && action.selector != null && locatorRadioButton.Checked)
                action.target = locatorsComboBox.SelectedIndex.ToString();

            else if (selectorRadioButton.Checked)
                action.target = "selector";

            return action;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            je?.Close();
            EditUserControl euc = (EditUserControl) Parent?.Parent;
            Parent?.Controls.Remove(this);
            euc?.UpdateAllActionUpDownButtons();
        }

        private void SetEnableModifications(bool state)
        {
            typeComboBox.Enabled = state;
            locatorsComboBox.Enabled = state;
            selectorTextBox.Enabled = state;
            jsonEditButton.Enabled = state;
            valueTextBox.Enabled = state;
        }

        private void jsonEditButton_Click(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(action, ConfigManager.JsonSettings);
            je = new JsonEditor();
            je.Closing += (o, args) =>
            {
                action = JsonConvert.DeserializeObject(je.GetEditorText(), ConfigManager.JsonSettings);
                BindActionToUi();
                SetEnableModifications(true);
            };

            SetEnableModifications(false);
            je.SetEditorText(json);
            je.Show();
        }

        private void typeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            action.type = typeComboBox.SelectedItem;
            ((EditUserControl)Parent?.Parent)?.ApplyFilter();
        }


        private void selectorTextBox_TextChanged(object sender, EventArgs e)
        {

            action.selector = selectorTextBox.Text;
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            action.value = valueTextBox.Text;
        }

        private bool isPuppeteerEvent(string typeName)
        {
            return typeName.ToLower().StartsWith("page");
        }

        private bool isViewportEvent(string typeName)
        {
            return !isPuppeteerEvent(typeName);
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            int currentIdx = Parent.Controls.GetChildIndex(this);
            int previousIdx = GetPreviousVisiblePosition(currentIdx);
            Parent.Controls.SetChildIndex(this, previousIdx);

            upButton.Enabled = GetPreviousVisiblePosition(previousIdx) != -1;
            

            downButton.Enabled = true;

            if (currentIdx - previousIdx > 1)
                Parent.Controls.SetChildIndex(Parent.Controls[previousIdx + 1], currentIdx);


            ActionUserControl auc = (ActionUserControl)Parent.Controls[currentIdx];
            auc.upButton.Enabled = true;
            auc.downButton.Enabled = GetNextVisiblePosition(currentIdx) != -1;

        }

        private int GetNextVisiblePosition(int currentIdx)
        {
            int nextPosition = currentIdx + 1;
            while (nextPosition < Parent.Controls.Count)
            {
                if (Parent.Controls[nextPosition].Visible)
                    return nextPosition;

                if (!Parent.Visible)
                    return nextPosition;

                ++nextPosition;
            }

            return -1;
        }

        private int GetPreviousVisiblePosition(int currentIdx)
        {
            int previousPosition = currentIdx - 1;
            while (previousPosition >= 0)
            {
                if (Parent.Controls[previousPosition].Visible)
                    return previousPosition;

                if (!Parent.Visible)
                    return previousPosition;

                --previousPosition;

            }

            return -1;
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int currentIdx = Parent.Controls.GetChildIndex(this);
            int nextIdx = GetNextVisiblePosition(currentIdx);
            Parent.Controls.SetChildIndex(this, nextIdx);

            downButton.Enabled = GetNextVisiblePosition(nextIdx) != -1;


            upButton.Enabled = true;

            if (nextIdx - currentIdx > 1)
                Parent.Controls.SetChildIndex(Parent.Controls[nextIdx + 1], currentIdx);


            ActionUserControl auc = (ActionUserControl)Parent.Controls[currentIdx];
            auc.downButton.Enabled = true;
            auc.upButton.Enabled = GetPreviousVisiblePosition(currentIdx) != -1;
        }

        public void UpdateUpDownButtons()
        {
            int currentIdx = Parent.Controls.GetChildIndex(this);
            int previousIdx = GetPreviousVisiblePosition(currentIdx);
            upButton.Enabled = previousIdx > -1;

            int nextIdx = GetNextVisiblePosition(currentIdx);
            downButton.Enabled = nextIdx < Parent.Controls.Count && nextIdx > -1;
        }

        public void SetSelected(bool c)
        {
            selectCheckBox.Checked = c;
        }

        private void selectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((EditUserControl) Parent?.Parent)?.ActionUserControlCheckedChanged(this);
            ((EditUserControl)Parent?.Parent)?.ApplyFilter();
            uiConfig.Selected = selectCheckBox.Checked;
        }

        private void ActionChanged(object sender, EventArgs e)
        {
            ((EditUserControl)Parent?.Parent)?.ActionUserControlEnableCheckedChanged(this);
            ((EditUserControl)Parent?.Parent)?.ApplyFilter();

            if ((sender as CheckBox)?.Name == "enabledCheckBox")
            {
                uiConfig.Enabled = enabledCheckBox.Checked;
            }
            else if ((sender as RadioButton)?.Name == "locatorRadioButton" && locatorRadioButton.Checked)
            {
                uiConfig.Target = Target.Locator;
            }
            else if ((sender as RadioButton)?.Name == "selectorRadioButton" && selectorRadioButton.Checked)
            {
                uiConfig.Target = Target.Selector;
            }
        }

        private void locatorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiConfig.SelectedLocatorIndex = locatorsComboBox.SelectedIndex;
        }

        public void BindAction(dynamic a)
        {
            action = a;
            uiConfig = new UiConfig();
            BindActionToUi();
            ApplyUi();
        }
    }
}
