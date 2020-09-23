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

namespace Frontend
{
    public partial class MainForm : Form
    {
        public enum AppMode
        {
            List, Edit
        }

        private  AppMode appState = AppMode.List;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: odesli ahoj

            using (var pair = new PairSocket("@tcp://127.0.0.1:3000")) // connect
            {
                // Send a message from the client socket
                pair.SendFrame("launch");
                pair.SendFrame("test");
                Console.WriteLine(pair.ReceiveFrameString());
                

                //// Receive the message from the server socket
                //string m1 = server.ReceiveFrameString();
                //Console.WriteLine("From Client: {0}", m1);

                //// Send a response back from the server
                //server.SendFrame("Hi Back");

                //// Receive the response from the client socket
                //string m2 = client.ReceiveFrameString();
                //Console.WriteLine("From Server: {0}", m2);
            }
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


            Thumbnail test = new Thumbnail
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Id = 5,
                Name = "Test",
                Websites = new List<Uri> { new Uri("https://grizly.cz"), new Uri("https://seznam.cz") }
            };
            ThumbnailUserControl tuc = new ThumbnailUserControl();
            tuc.BindThumbnail(test);
            thumbnailsFlowLayoutPanel.Controls.Add(tuc);
        }

        private void SwitchToEditMode(Thumbnail t)
        {
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

            if (IsNewRecording(t))
            {
                currentEdit.Recording = new Recording();
                currentEdit.Thumbnail = t;

            }
            else //editing an existing recording
            {
                string recordingJson = File.ReadAllText($"recordings/{t.Id}.json");
                currentEdit.Recording = JsonConvert.DeserializeObject<Recording>(recordingJson, ConfigManager.JsonSettings);
            }

            //editUserControl

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
            njc.Show();
        }
    }
}
