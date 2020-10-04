using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend.Forms;
using Frontend.UserControls;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frontend
{
    public partial class MainForm : Form
    {
        List<Form> openedSettingForms = new List<Form>();
        public enum AppMode
        {
            List, Edit
        }

        private AppMode appState = AppMode.List;
        private CurrentEdit currentEdit;

        public  AppMode AppState
        {
            get { return appState; }
            set
            {
                appState = value;
                UiChange();
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void recorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecorderSettingsForm rs = new RecorderSettingsForm();
            rs.Closing += (o, args) =>
            {
                RecorderConfiguration rc = rs.ExportRecorderOptions();
                ConfigManager.SavePuppeteerConfiguration(rc.PuppeteerOptions);
                ConfigManager.SaveRecordedEventsConfiguration(rc.RecordedEvents);
              
            };
            openedSettingForms.Add(rs);
            rs.Show();
        }

        private void codeGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeGeneratorSettingsForm cgs = new CodeGeneratorSettingsForm();
            cgs.BindCodeGeneratorOptions(ConfigManager.GetCodeGeneratorOptions());

            cgs.Closing += (o, args) =>
            {
                CodeGeneratorOptions cgo = cgs.ExportCodeGeneratorOptions();
                ConfigManager.SaveCodeGeneratorOptions(cgo);
            };
            openedSettingForms.Add(cgs);
            cgs.Show();
        }

        private void playerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerForm pf = new PlayerForm();
            pf.BindOptions(ConfigManager.GetPlayerOptions());
            pf.Closing += (o, args) =>
            {
                PlayerOptions po = pf.ExportOptions();
                ConfigManager.SavePlayerOptions(po);
            };
            openedSettingForms.Add(pf);
            pf.Show();
        }

        private void LoadThumbnailsUi(List<Thumbnail> thumbnails)
        {
            foreach(Thumbnail t in thumbnails)
            {
                ThumbnailUserControl tuc = new ThumbnailUserControl();
                tuc.BindThumbnail(t);
                thumbnailsFlowLayoutPanel.Controls.Add(tuc);
            }
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            editUserControl.InitMain(this);
            List<Thumbnail> thumbnails = ThumbnailManager.Init();
            LoadThumbnailsUi(thumbnails);
        }

        public void SwitchToEditMode(Thumbnail t)
        {
            openedSettingForms.ForEach(f=>f.Close());
            openedSettingForms.Clear();

            if (currentEdit == null)
                currentEdit = new CurrentEdit();

            currentEdit.Config = new Configuration
            {
                PuppeteerConfig = ConfigManager.GetPuppeteerConfiguration(),
                CodeGeneratorConfig = ConfigManager.GetCodeGeneratorOptions(),
                PlayerOptions = ConfigManager.GetPlayerOptions(),
                NodeJsOptions = ConfigManager.GetNodeJsOptions(),
                RecordedEvents = ConfigManager.GetRecordedEventsOptions()
            };
            currentEdit.Thumbnail = t;
            if (IsNewRecording(t))
                currentEdit.Recordings = new List<Recording>();
            
            else //editing an existing recording
            {
                string recordingJson = File.ReadAllText($"recordings/{t.Id}.json");
                dynamic recordings = JsonConvert.DeserializeObject(recordingJson, ConfigManager.JsonSettings);
                currentEdit.StartupHints = recordings.StartupHints;
                currentEdit.Recordings = (List<Recording>) ((JArray) recordings.Recordings).ToObject(typeof(List<Recording>));
            }

            editUserControl.BindEdit(currentEdit);

            AppState = AppMode.Edit;
        }


        private bool IsNewRecording(Thumbnail thumbnail)
        {
            return !File.Exists($"recordings/{thumbnail.Id}.json");
        }

        private void UiChange()
        {
            if (AppState == AppMode.List)
            {
                SetListUiVisibility(true);
                SetEditUiVisiblity(false);

                thumbnailsFlowLayoutPanel.Controls.Clear();
                LoadThumbnailsUi(ThumbnailManager.LoadThumbnails());
            }
            else //Edit
            {
                SetListUiVisibility(false);
                SetEditUiVisiblity(true);
            }
        }

        private void SetEditUiVisiblity(bool state)
        {
            editUserControl.Visible = state;
            optionsToolStripMenuItem.Enabled = !state;
        }

        private void SetListUiVisibility(bool state)
        {
            addNewRecordingButton.Visible = state;
            thumbnailsFlowLayoutPanel.Visible = state;
            nameLabel.Visible = state;
            websitesLabel.Visible = state;
            createdLabel.Visible = state;
            updatedLabel.Visible = state;
            optionsToolStripMenuItem.Enabled = !state;
        }

        private void addNewRecordingButton_Click(object sender, EventArgs e)
        {
            Thumbnail t = ThumbnailManager.NewThumbnail();
            SwitchToEditMode(t);
        }

        private void nodejsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeJsConfig njc = new NodeJsConfig();
            njc.Closing += (o, args) =>
            {
                ConfigManager.SaveNodeJsOptions(njc.ExportNodeJsOptions());
            };
            njc.BindNodeJsOptions(ConfigManager.GetNodeJsOptions());
            openedSettingForms.Add(njc);
            njc.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            editUserControl.NodeJsProcess?.Kill();
        }
    }
}
