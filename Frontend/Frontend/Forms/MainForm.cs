using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Frontend.Forms;
using Frontend.UserControls;
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

            PerformSettingsChecks();
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
            {
                currentEdit.Recordings = new List<Recording>();
                currentEdit.NextAvailableId = 0;
            }

            else //editing an existing recording
            {
                string recordingJson = File.ReadAllText($"recordings/{t.Id}.json");
                dynamic recordings = JsonConvert.DeserializeObject(recordingJson, ConfigManager.JsonSettings);
                currentEdit.StartupHints = recordings.StartupHints;
                currentEdit.Recordings = (List<Recording>) ((JArray) recordings.Recordings).ToObject(typeof(List<Recording>));
                currentEdit.NextAvailableId = recordings.NextId;
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

            menuStrip.Visible = !state;
            //optionsToolStripMenuItem.Enabled = !state;
        }

        private void SetListUiVisibility(bool state)
        {
            addNewRecordingButton.Visible = state;
            thumbnailsFlowLayoutPanel.Visible = state;
            nameLabel.Visible = state;
            websitesLabel.Visible = state;
            createdLabel.Visible = state;
            updatedLabel.Visible = state;
            menuStrip.Visible = !state;
            nodeInterpreterVersionLabel.Visible = state;
        }

        private void addNewRecordingButton_Click(object sender, EventArgs e)
        {
            Thumbnail t = ThumbnailManager.NewThumbnail();
            SwitchToEditMode(t);
        }

        private void nodejsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeJsConfig njc = new NodeJsConfig(this);
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
            if(editUserControl.NodeJsProcess != null && !editUserControl.NodeJsProcess.HasExited)
                editUserControl.NodeJsProcess.Kill();
        }

        private void SetUiBasedOnSettings(bool state)
        {
            thumbnailsFlowLayoutPanel.Enabled = state;
            addNewRecordingButton.Enabled = state;
        }

        private void UiSafeOperation(Action a)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => a()));

            else
                a();
        }

        public void PerformSettingsChecks()
        {
            NodeJsOptions njo = ConfigManager.GetNodeJsOptions();
            Process p = new Process();
            p.EnableRaisingEvents = true;
            ProcessStartInfo psi = new ProcessStartInfo(njo.InterpreterPath, "-v");
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            p.Exited += (sender, args) =>
            {
                string nodeV = p.StandardOutput.ReadToEnd();

                if (nodeV.StartsWith("v"))
                {
                    UiSafeOperation(() =>
                    {
                        nodeInterpreterVersionLabel.Text = $"node -v: {nodeV}";
                        SetUiBasedOnSettings(true);
                    });
                }
                else
                    UiSafeOperation(NodeInterpterNotWorking);
                    
                
            };
            p.StartInfo = psi;
            try
            {
                p.Start();
            }
            catch (Exception)
            {
                UiSafeOperation(NodeInterpterNotWorking);
            }
        }

        private void NodeInterpterNotWorking()
        {
            nodeInterpreterVersionLabel.Text = "node -v: Problems with starting node interpreter, check node.js interpreter path in options.";
            SetUiBasedOnSettings(false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (AppState == AppMode.Edit)
            {
                if (keyData == (Keys.Shift | Keys.Delete))
                {
                    editUserControl.DeleteRequested();
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
