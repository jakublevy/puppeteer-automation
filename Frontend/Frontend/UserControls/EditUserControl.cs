using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Frontend.Forms;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;


namespace Frontend.UserControls
{
    public partial class EditUserControl : UserControl
    {
        private MainForm mf;
        private State recorderState = State.Disconnected;

        private Timer recordingTimer;

        public State RecorderState
        {
            get { return recorderState; }
            set { 
                recorderState = value;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            if (recorderState == State.Connected)
            {
                browserConnection.Text = "Disconnect/Stop Browser";
                recordButton.Enabled = true;
            }
            else if (recorderState == State.Disconnected)
            {
                browserConnection.Text = "Connect/Start Browser";
                recordButton.Enabled = false;
            }
            else if (recorderState == State.Recording)
            {
                recordButton.Text = "Stop Recording";
            }
        }

        private PairSocket pair = null;
        private CurrentEdit edit;

        public enum State
        {
            Connected, Disconnected, Recording
        }
        public EditUserControl()
        {
            InitializeComponent();
        }

        public void InitMain(MainForm m)
        {
            mf = m;
        }

        public void BindEdit(CurrentEdit ce)
        {
            edit = ce;

            nameTextBox.DataBindings.Clear();
            nameTextBox.DataBindings.Add("Text", edit.Thumbnail, "Name");
        }

        private void saveAndExitButton_Click(object sender, EventArgs e)
        {
            //TODO: save changes to name and captures recordings

            mf.AppState = MainForm.AppMode.List;
        }

        private void StartNodeJsProcess()
        {
            string workingDir = Path.GetDirectoryName(Path.GetDirectoryName(edit.Config.NodeJsOptions.NodeJsEntryPoint));

            Process p = new Process();
            p.StartInfo.WorkingDirectory = workingDir;
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = edit.Config.NodeJsOptions.InterpreterPath;
            p.StartInfo.Arguments = edit.Config.NodeJsOptions.NodeJsEntryPoint;
            try
            {
              //  p.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Received(string s)
        {
            string response = pair.ReceiveFrameString();
            return response == s;
        }

        private void browserConnection_Click(object sender, EventArgs e)
        {
            if (recorderState == State.Disconnected)
            {
                try
                {
                    StartNodeJsProcess();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(pair == null)
                    pair = new PairSocket("@tcp://127.0.0.1:3000");

                string eventsToRecord = JsonConvert.SerializeObject(GetEventsToRecord());
                pair.SendFrame("setEventsToRecord");
                pair.SendFrame(eventsToRecord);

                if (edit.Config.PuppeteerConfig is LaunchPuppeteerOptions lpo)
                {
                    pair.SendFrame("launch");
                    pair.SendFrame(JsonConvert.SerializeObject(lpo, ConfigManager.JsonSettings));

                }
                else if (edit.Config.PuppeteerConfig is ConnectPuppeteerOptions cpo)
                {
                    pair.SendFrame("connect");
                }

                if (Received("OK"))
                    RecorderState = State.Connected;
                
                
            }
            else if (recorderState == State.Connected)
            {
                if (edit.Config.PuppeteerConfig is LaunchPuppeteerOptions lpo)
                {
                    pair.SendFrame("close");
                }
                else if (edit.Config.PuppeteerConfig is ConnectPuppeteerOptions cpo)
                {
                    pair.SendFrame("disconnect");
                }

                if (Received("OK"))
                    RecorderState = State.Disconnected;
            }
        }

        private List<string> GetEventsToRecord()
        {
            List<string> events = new List<string>();
            foreach (PropertyInfo pi in edit.Config.RecordedEvents.GetType().GetProperties())
            {
                if ((bool) pi.GetValue(edit.Config.RecordedEvents) && pi.CanWrite)
                {
                    if (pi.Name.StartsWith("page", StringComparison.OrdinalIgnoreCase))
                        events.Add(char.ToLower(pi.Name[0]) + pi.Name.Substring(1));
                    else
                        events.Add(pi.Name.ToLower());
                }
            }
            return events;
        }

        private bool IsBrowserConnected()
        {
            if (pair == null)
                return false;

            pair.SendFrame("browserConnectionStatus");
            return bool.Parse(pair.ReceiveFrameString());
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            bool connectionStatus = IsBrowserConnected();
            if (connectionStatus && recorderState != State.Recording)
            {
                recordingTimer = new Timer(100);
                recordingTimer.Elapsed += RecordingTimerOnElapsed;
                recordingTimer.Enabled = true;

                pair.SendFrame("start");

                RecorderState = State.Recording;
            }
            else if(!connectionStatus)
            {
                RecorderState = State.Disconnected;
                MessageBox.Show("Connection to Node.js app lost", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (connectionStatus && recorderState == State.Recording) //Stop recording
            {
                recordingTimer.Stop();
                pair.SendFrame("stop");
                recorderState = State.Connected;
            }
        }

        private void RecordingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            recordingTimer.Stop();

            if(pair == null) 
                return;

            string json = pair.ReceiveFrameString();
            dynamic action = JsonConvert.DeserializeObject(json, ConfigManager.JsonSettings);
            if (action.type != "startupHints")
            {
                ActionUserControl auc = new ActionUserControl();
                auc.BindAction(action);

                if (actionsFlowLayoutPanel.InvokeRequired)
                    actionsFlowLayoutPanel.Invoke(new MethodInvoker(() => actionsFlowLayoutPanel.Controls.Add(auc)));
            }

            edit.Recording.Actions.Add(action);

            recordingTimer.Start();
        }
    }
}
