using System;
using System.Windows.Forms;
using Frontend.Forms;
using Newtonsoft.Json;

namespace Frontend.UserControls
{
    /// <summary>
    /// This user control corresponds to a recorded action.
    /// </summary>
    public partial class ActionUserControl : UserControl
    {
        //the underlying action JSON data
        private dynamic action;

        //additional ui configuration
        //e.g. is action enabled, is action selected, ...
        private UiConfig uiConfig;

        public int Id { get; private set; }

        //for purposes of editing action variable
        private JsonEditor je = null;


        /// <summary>
        /// true if action is selected,
        /// false otherwise
        /// </summary>
        public bool Selected => selectCheckBox.Checked;

        /// <summary>
        /// true if action is enabled (for output)
        /// false otherwise
        /// </summary>
        public bool EnabledForOutput => enabledCheckBox.Checked;

        public ActionUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds the supplied parameters into action, uiConfig, and Id variables.
        /// Then it calls underlying methods that make sure the changes are reflected into UI.
        /// </summary>
        public void BindRecording(Recording r, int id)
        {
            action = r.Action;
            uiConfig = r.UiConfig;
            Id = id;
            BindActionToUi();
            ApplyUi();
        }

        /// <summary>
        /// Reflects the changes that were made to uiConfig (from code) into UI
        /// </summary>
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

        /// <summary>
        /// Reflects the changes that were made to action from code into UI.
        /// </summary>
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

        /// <summary>
        /// Exports the current UI into Recording object.
        /// </summary>
        public Recording ExportRecordingForSave()
        {
            return new Recording {Action = action, UiConfig = uiConfig, Id = Id};
        }

        /// <summary>
        /// Calls ExportActionForOutputImpl in a safe manner.
        /// </summary>
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

        /// <summary>
        /// Exports the current UI into an action json object.
        /// The exported object contains the property "target" describing the chosen identifier as filled in UI.
        /// </summary>
        public dynamic ExportActionForOutputImpl()
        {
            if (action.locators != null && action.locators.Count != 0 && locatorRadioButton.Checked)
            {
                if (locatorsComboBox.SelectedIndex == -1 && locatorsComboBox.Text != "")
                    action.target = locatorsComboBox.Text;
                
                else
                    action.target = locatorsComboBox.SelectedIndex.ToString();
            }

            if (selectorRadioButton.Checked)
                action.target = "selector";

            return action;
        }

        /// <summary>
        /// Remove this action on "Delete" button click
        /// </summary>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            je?.Close();
            EditUserControl euc = (EditUserControl) Parent?.Parent;
            Parent?.Controls.Remove(this);
            euc?.UpdateAllActionUpDownButtons();
        }

        /// <summary>
        /// Enables/disables UI modification.
        /// </summary>
        private void SetEnableModifications(bool state)
        {
            typeComboBox.Enabled = state;
            locatorsComboBox.Enabled = state;
            selectorTextBox.Enabled = state;
            jsonEditButton.Enabled = state;
            valueTextBox.Enabled = state;
        }

        /// <summary>
        /// On the "JSON" button click, this method opens a JSON editor that allows the user to modify JSON data in the action variable.
        /// </summary>
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

        /// <summary>
        /// This method changes an action type (click -> dblclick and vice versa) after a change in UI.
        /// Filter is reapplied too.
        /// </summary>
        private void typeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            action.type = typeComboBox.SelectedItem;
            ((EditUserControl)Parent?.Parent)?.ApplyFilter();
        }


        /// <summary>
        /// Updates selector value after a change in UI.
        /// </summary>
        private void selectorTextBox_TextChanged(object sender, EventArgs e)
        {
            action.selector = selectorTextBox.Text;
        }

        /// <summary>
        /// Updates action value after a change in UI.
        /// </summary>
        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            action.value = valueTextBox.Text;
        }


        // The following region contains the functionality of ↑ button and ↓ button.
        // These buttons change the order of actions for replay.
        #region Actions order functionality


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
        #endregion


        /// <summary>
        /// Sets the selection state of the action.
        /// </summary>
        public void SetSelected(bool c)
        {
            selectCheckBox.Checked = c;
        }

        /// <summary>
        /// UI logic when the action is selected or deselected.
        /// </summary>
        private void selectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((EditUserControl) Parent?.Parent)?.ActionUserControlCheckedChanged(this);
            ((EditUserControl)Parent?.Parent)?.ApplyFilter();
            uiConfig.Selected = selectCheckBox.Checked;
        }

        /// <summary>
        /// Reflecting UI changes into variables.
        /// Reapplying filter too.
        /// </summary>
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

        /// <summary>
        /// Reflecting UI changes into variables.
        /// </summary>
        private void locatorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiConfig.SelectedLocatorIndex = locatorsComboBox.SelectedIndex;
        }

        /// <summary>
        /// Binds JSON data with id only without any ui configuration.
        /// Default values will be used for ui configuration.
        /// </summary>
        public void BindAction(dynamic a, int id)
        {
            action = a;
            uiConfig = new UiConfig();
            this.Id = id;
            BindActionToUi();
            ApplyUi();
        }
    }
}
