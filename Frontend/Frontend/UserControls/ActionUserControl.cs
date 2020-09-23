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
        private JsonEditor je = null;
        public ActionUserControl()
        {
            InitializeComponent();
        }

        public void BindAction(dynamic a)
        {
            action = a;
            BindActionToUi();

            //TODO: put action into controls
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

            if (action.type == "dblclick")
                typeComboBox.SelectedIndex = 1;
            else
                typeComboBox.SelectedIndex = 0;

            if (action.locators != null && action.locators.Count > 0)
            {
                foreach (var locator in action.locators)
                {
                    locatorsComboBox.Items.Add(locator.locator);
                    if (locator.use != null && locator.use)
                        locatorsComboBox.SelectedIndex = locatorsComboBox.Items.Count - 1;
                }

                if (oldSelectedLocatorIndex != -1)
                    locatorsComboBox.SelectedIndex = oldSelectedLocatorIndex;

                if (locatorsComboBox.SelectedIndex == -1)
                    locatorsComboBox.SelectedIndex = 0;

                
            }
            selectorTextBox.Text = action.selector;
            valueTextBox.Text = action.value;

        }

        public dynamic ExportAction()
        {
            //TODO:
            return new {TODO = "TODO"};
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

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
        }


        private void selectorTextBox_TextChanged(object sender, EventArgs e)
        {
            action.selector = selectorTextBox.Text;
        }
    }
}
