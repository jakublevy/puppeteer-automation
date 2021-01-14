using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Frontend.Forms;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;


namespace Frontend.UserControls
{
    public partial class EditUserControl : UserControl
    {
        private MainForm mf;

        //state of edit - at the start the app is disconnected from the browser
        private State workingState = State.Disconnected;
        public State WorkingState
        {
            get { return workingState; }
            set
            {
                workingState = value;
                UpdateUi();
            }
        }

        //When we are recording, this task listens for incoming actions.
        private Task recordingTask;

        //When we are replaying, this task sends actions to the backend for replay.
        private Task replayTask;

        //machinery used to request a cancel of running either replayTask or recordingTask
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken cancelToken;

        //Process associated with backend
        public Process NodeJsProcess;

        //Filter with its UI counterpart
        private Filter activeFilter = null;
        private FilterForm filterForm = null;

        //UI that displays the state of replaying
        private ReplayViewForm replayViewForm;

        //AUC that is highlighted when it's being replayed
        private ActionUserControl runningNowHighlightedAuc;

        //Last AUC that is highlighted to show the connection between the error message and AUC
        private ActionUserControl oldErrorHighlightedAuc;

        //defines whether the replay should be canceled as soon as possible
        private bool forceCancelReplay = false;

        //lock to make sure than only one thread at a time changes the highlighting of actions
        private object colorChangeLck = new object();

        //NetMQ socket for communication with Backend.
        private PairSocket sock = null;

        //This contains the recording this code works with.
        //It also contains other things like configuration.
        private CurrentEdit edit;

        public enum State
        {
            Connected, //connected to the backend and a browser
            Disconnected, //disconnect from the backend
            Recording //currently recording
        }
        public EditUserControl()
        {
            InitializeComponent();
        }

        public void InitMain(MainForm m)
        {
            mf = m;
        }

        /// <summary>
        /// Binds new recording and sets UI and filter accordingly.
        /// </summary>
        /// <param name="ce"></param>
        public void BindEdit(CurrentEdit ce)
        {
            edit = ce;
            nameTextBox.DataBindings.Clear();
            nameTextBox.DataBindings.Add("Text", edit.Thumbnail, "Name");
            ce.Recordings.ForEach(r => LoadRecording(r));
            FilterChanged(activeFilter);
        }

        /// <summary>
        /// Called whenever filter was changed.
        /// Applies the new active filter and updates UI.
        /// </summary>
        public void FilterChanged(Filter f)
        {
            if (f == null)
                return;

            activeFilter = f;

            if (f.EventTypes.Count == 0 && f.Status.Count == 0 && f.Targets.Count == 0)
            {
                filterLabel.Text = "Filter Disabled";
                filterLabel.ForeColor = Color.Red;
            }
            else
            {
                filterLabel.Text = "Filter Enabled";
                filterLabel.ForeColor = Color.ForestGreen;
            }
            ApplyFilter();
        }

        /// <summary>
        /// This method applies the activeFilter variable.
        /// After this function is executed, actions will be filtered with respect to the activeFilter.
        /// </summary>
        public void ApplyFilter()
        {
            if (activeFilter == null)
                return;

            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                dynamic action = auc.ExportActionForOutput();

                //event types
                if (activeFilter.EventTypes.FindIndex(x => NormalizeEventName(x) == action.type.ToString()) != -1)
                {
                    auc.Visible = false;
                    continue;
                }

                if (action.target == "selector" && activeFilter.Targets.Contains("Selector"))
                {
                    auc.Visible = false;
                    continue;
                }

                if (action.target != "selector" && activeFilter.Targets.Contains("Locator"))
                {
                    auc.Visible = false;
                    continue;
                }

                if (auc.EnabledForOutput && activeFilter.Status.Contains("Enabled"))
                {
                    auc.Visible = false;
                    continue;
                }

                if (!auc.EnabledForOutput && activeFilter.Status.Contains("Disabled"))
                {
                    auc.Visible = false;
                    continue;
                }

                if (auc.Selected && activeFilter.Status.Contains("Selected"))
                {
                    auc.Visible = false;
                    continue;
                }


                if (!auc.Selected && activeFilter.Status.Contains("Not Selected"))
                {
                    auc.Visible = false;
                    continue;
                }

                auc.Visible = true;
            }

            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
                auc.UpdateUpDownButtons();
        }

        /// <summary>
        /// Returns true if there are some actions to generate code with/replay.
        /// </summary>
        private bool SomeActionsSelectedForProcessing()
        {
            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                if (enabledRadioButton.Checked && auc.EnabledForOutput)
                    return true;
                if (selectedRadioButton.Checked && auc.Selected)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Updates UI to match the current workingState.
        /// </summary>
        private void UpdateUi()
        {
            if (workingState == State.Connected)
            {
                browserConnection.Text = "Disconnect/Stop Browser";
                recordButton.Text = "Start Recording";
                recordButton.Enabled = true;
                UpdateProcessButtons();
            }
            else if (workingState == State.Disconnected)
            {
                browserConnection.Text = "Connect/Start Browser";
                recordButton.Enabled = false;
                optimizeButton.Enabled = false;
                codeGenButton.Enabled = false;
                replayButton.Enabled = false;

                InterruptTasks();
                if (NodeJsProcess != null && !NodeJsProcess.HasExited)
                    NodeJsProcess?.Kill();
                sock?.Close();
                sock = null;

            }
            else if (workingState == State.Recording)
            {
                recordButton.Text = "Stop Recording";
                optimizeButton.Enabled = false;
                codeGenButton.Enabled = false;
                replayButton.Enabled = false;
            }
        }

        /// <summary>
        /// Mostly UI logic of "Save & Exit" Button
        /// </summary>
        private void saveAndExitButton_Click(object sender, EventArgs e)
        {
            if (WorkingState == State.Recording)
            {
                DialogResult dr = MessageBox.Show(
                     "The recording is still running. Do you want to continue?",
                     "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    return;

                recordButton.PerformClick();
            }
            else if (WorkingState == State.Connected)
            {
                DialogResult dr = MessageBox.Show("The connection to browser is still active. Do you want to continue?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    return;
                browserConnection.PerformClick();
            }

            //Update recording information
            edit.Thumbnail.Name = nameTextBox.Text;
            edit.Thumbnail.Updated = DateTime.Now;
            edit.Recordings = GetAllActions();
            edit.Thumbnail.Websites.UnionWith(ScrapeWebsitesFromRecordings(edit.Recordings));

            //Save
            RecordingManager.SaveCurrentEdit(edit);

            //Return to the main form
            mf.AppState = MainForm.AppMode.List;
            replayViewForm?.Close();
            actionsFlowLayoutPanel.Controls.Clear();
            filterForm?.Close();
        }

        /// <summary>
        /// Gets websites of this recording (from pageUrlChanged action).
        /// </summary>
        private HashSet<string> ScrapeWebsitesFromRecordings(List<Recording> recordings)
        {
            HashSet<string> uris = new HashSet<string>();
            IEnumerable<dynamic> urlActions = recordings.Select(x => x.Action).Where(x => x.type == "pageUrlChanged");

            string url;
            foreach (dynamic a in urlActions)
            {
                url = new Uri(a.newUrl.ToString()).Authority;
                uris.Add(url);
            }

            return uris;
        }

        /// <summary>
        /// Returns all actions, each action in addition to JSON data contains ui config and id.
        /// </summary>
        private List<Recording> GetAllActions()
        {
            List<Recording> recordings = new List<Recording>();
            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
                recordings.Add(auc.ExportRecordingForSave());

            return recordings;
        }

        /// <summary>
        /// Starts the Backend
        /// </summary>
        private void StartNodeJsProcess()
        {
            string absolutePath = Path.GetFullPath(edit.Config.NodeJsOptions.NodeJsEntryPoint);
            string workingDir = Path.GetDirectoryName(Path.GetDirectoryName(absolutePath));
            string relativeEnd = Path.GetFileName(Path.GetDirectoryName(absolutePath)) + "\\" + Path.GetFileName(absolutePath);
            NodeJsProcess = new Process();
            NodeJsProcess.Exited += (sender, args) =>
            {
                WorkingState = State.Disconnected;
            };
            NodeJsProcess.StartInfo.WorkingDirectory = workingDir;
            NodeJsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            NodeJsProcess.StartInfo.UseShellExecute = false;
            NodeJsProcess.StartInfo.RedirectStandardError = true;
            NodeJsProcess.StartInfo.RedirectStandardInput = true;
            NodeJsProcess.StartInfo.RedirectStandardOutput = true;

            NodeJsProcess.StartInfo.FileName = edit.Config.NodeJsOptions.InterpreterPath;
            NodeJsProcess.StartInfo.Arguments = relativeEnd;
            NodeJsProcess.Start();
        }

        /// <summary>
        /// If Backend is not running:
        ///    Connects/launches the browser
        /// If Backend is running
        ///    Disconnect/closes the browser and quits Backend process.
        /// </summary>
        private void browserConnection_Click(object sender, EventArgs e)
        {
            forceCancelReplay = false;
            if (workingState == State.Disconnected)
            {
                try
                {
                    StartNodeJsProcess();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Node.js process cannot be started. Check whether Node.js interpreter path is correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (sock == null)
                    sock = new PairSocket("@tcp://127.0.0.1:3000");


                string eventsToRecord = JsonConvert.SerializeObject(GetEventsToRecord());
                bool b = sock.TrySendFrame(new TimeSpan(0, 0, 0, 0, 3000), "setEventsToRecord");
                if (!b)
                {
                    HandleIncorrectProcess();
                    return;
                }
                sock.SendFrame(eventsToRecord);

                if (edit.Config.PuppeteerConfig is LaunchPuppeteerOptions lpo)
                {
                    sock.SendFrame("launch");
                    sock.SendFrame(JsonConvert.SerializeObject(lpo, ConfigManager.JsonSettings));


                }
                else if (edit.Config.PuppeteerConfig is ConnectPuppeteerOptions cpo)
                {
                    sock.SendFrame("connect");
                    sock.SendFrame(JsonConvert.SerializeObject(cpo, ConfigManager.JsonSettings));

                }

                string response;
                bool received = sock.TryReceiveFrameString(new TimeSpan(0, 0, 0, 0, 8000), out response);
                if (received)
                {
                    if (response == "ACK")
                        WorkingState = State.Connected;

                    else
                    {
                        if (edit.Config.PuppeteerConfig is LaunchPuppeteerOptions)
                        {
                            MessageBox.Show("Could not launch browser, check whether chrome/chromium is installed or correct path was supplied in options", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            HandleIncorrectProcess();
                        }
                        else
                        {
                            MessageBox.Show("Could not connect to browser, check IP a Port number and whether the browser process is running with --remote-debugging-port=PORT_N", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            HandleIncorrectProcess();
                        }
                    }

                }
                else
                {
                    HandleIncorrectProcess();
                }
            }
            else if (workingState == State.Connected || workingState == State.Recording)
            {
                forceCancelReplay = true;
                WorkingState = State.Disconnected;
                replayViewForm?.ForceClose();
            }
        }

        private void HandleIncorrectProcess()
        {
            try
            {
                NodeJsProcess?.Kill();
            }
            catch(Exception){}

            MessageBox.Show("Node.js app is not responding. Check if node interpreter is running correct code (check backend path or remote connection if used).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Request a cancel then waits until the end of the task.
        /// </summary>
        private void InterruptTasks()
        {
            cts?.Cancel();
            replayTask?.Wait();
            recordingTask?.Wait();
        }

        /// <summary>
        /// Gets the normalized name of all events that should be recorded.
        /// </summary>
        /// <returns></returns>
        private List<string> GetEventsToRecord()
        {
            List<string> events = new List<string>() { "urlHint", "startupHints" };
            foreach (PropertyInfo pi in edit.Config.RecordedEvents.GetType().GetProperties())
            {
                if ((bool)pi.GetValue(edit.Config.RecordedEvents) && pi.CanWrite)
                    events.Add(NormalizeEventName(pi.Name));

            }
            return events;
        }

        /// <summary>
        /// Converts given eventName string into a string with its first letter in lowercase. 
        /// </summary>
        private string NormalizeEventName(string eventName)
        {
            if (eventName.StartsWith("page", StringComparison.OrdinalIgnoreCase))
                return char.ToLower(eventName[0]) + eventName.Substring(1);

            return eventName.ToLower();
        }

        /// <summary>
        /// Cancels recordingTask if running and waits until it finishes.
        /// Then checks whether the connection to the Backend is up and returns corresponding boolean.
        /// </summary>
        private bool IsBrowserConnected()
        {
            if (sock == null)
                return false;

            if (recordingTask != null && recordingTask.Status == TaskStatus.Running)
            {
                cts.Cancel();
                recordingTask.Wait();
            }

            sock.SendFrame("browserConnectionStatus");

            string bStr;
            bool res = false;
            int counter = 0;
            while ((!sock.ReceiveFrameStringTimeout(out bStr, 100) || !bool.TryParse(bStr, out res)) && counter <= 4)
            {
                sock.SendFrame("browserConnectionStatus");
                ++counter;
            }

            if (counter > 4 || !res)
            {
                MessageBox.Show("Connection to browser was lost.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WorkingState = State.Disconnected;
                return false;
            }

            return res;
        }

        /// <summary>
        /// Starts or stops the recording by sending an appropriate message to Backend.
        /// </summary>
        private void recordButton_Click(object sender, EventArgs e)
        {
            bool connectionStatus = IsBrowserConnected();
            if (connectionStatus && workingState != State.Recording)
            {
                cts = new CancellationTokenSource();
                cancelToken = cts.Token;

                sock.SendFrame("start");

                WorkingState = State.Recording;
                recordingTask = Task.Factory.StartNew(RecordingTask, cancelToken);
            }
            else if (!connectionStatus)
            {
                WorkingState = State.Disconnected;
            }
            else if (connectionStatus && workingState == State.Recording) //Stop recording
            {
                WorkingState = State.Connected;
                sock.SendFrame("stop");
            }
        }

        /// <summary>
        /// Loads only the given action into UI. This method doesn't change any configuration of UI.
        /// </summary>
        private void LoadAction(dynamic action)
        {
            int id;
            if (action.ToString() == "True")
                return;
            if (action.type == "urlHint")
            {
                edit.Thumbnail.Websites.Add(new Uri(action.url.ToString()).Authority);
                id = -1;
            }
            else if (action.type == "startupHints")
            {
                edit.StartupHints = action;
                id = -1;
            }
            else
            {
                ActionUserControl auc = new ActionUserControl();
                auc.BindAction(action, edit.AllocateId());
                id = auc.Id;
                if (selectedAllCheckBox.CheckState == CheckState.Checked)
                    auc.SetSelected(true);


                UiSafeOperation(() =>
                {
                    actionsFlowLayoutPanel.Controls.Add(auc);

                    if (addAsFirst.Checked)
                        actionsFlowLayoutPanel.Controls.SetChildIndex(actionsFlowLayoutPanel.Controls[actionsFlowLayoutPanel.Controls.Count - 1], 0);

                    UpdateAllActionUpDownButtons();

                    actionsFlowLayoutPanel.ScrollControlIntoView(auc);
                });

            }

            if (addAsLast.Checked)
                edit.Recordings.Add(new Recording { Action = action, UiConfig = new UiConfig(), Id = id });
            else //addAsFirst.Checked
                edit.Recordings.Insert(0, new Recording { Action = action, UiConfig = new UiConfig(), Id = id });
        }

        /// <summary>
        /// Loads complete action with ui configuration into UI.
        /// </summary>
        private void LoadRecording(Recording r)
        {
            if (r.Action.type == "urlHint")
            {
                edit.Thumbnail.Websites.Add(new Uri(r.Action.url.ToString()).Authority);
            }
            else if (r.Action.type != "startupHints")
            {
                ActionUserControl auc = new ActionUserControl();
                auc.BindRecording(r, r.Id);
                if (selectedAllCheckBox.CheckState == CheckState.Checked)
                    auc.SetSelected(true);


                UiSafeOperation(() =>
                {
                    actionsFlowLayoutPanel.Controls.Add(auc);
                    UpdateAllActionUpDownButtons();
                });

            }
        }

        /// <summary>
        /// This method listens to captured actions. If a new action is found, UI is updated respectively.
        /// Execution of this method is started by recordingTask.
        /// </summary>
        private void RecordingTask()
        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (sock == null)
                    return;

                if (!sock.HasIn)
                    continue;

                if (sock.HasIn && cancelToken.IsCancellationRequested)
                    break;


                string json = null;
                if (!sock.TryReceiveFrameString(new TimeSpan(0, -0, 0, 0, 200), out json))
                {
                    continue;
                }

                dynamic action = JsonConvert.DeserializeObject(json, ConfigManager.JsonSettings);

                if (action.ToString() == "True")
                    continue;

                LoadAction(action);
            }
        }

        /// <summary>
        /// Revalidates Enabled property of ↑ button and ↓ button of every action.
        /// E.g. the first action should not allow clicking on ↑ button.
        /// E.g. the last action should not allow clicking on ↓ button.
        /// </summary>
        public void UpdateAllActionUpDownButtons()
        {
            foreach (ActionUserControl a in actionsFlowLayoutPanel.Controls)
                a.UpdateUpDownButtons();
        }

        private void selectedAllCheckBox_Click(object sender, EventArgs e)
        {
            bool b = true;
            if (selectedAllCheckBox.CheckState == CheckState.Indeterminate)
            {
                selectedAllCheckBox.Checked = false;
                b = false;
            }

            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                auc.SetSelected(b);
            }
        }

        /// <summary>
        /// Updates the actions selection checkbox:
        /// if all actions are selected, its value should be ✓
        /// if some actions are selected, its value should be ⬛
        /// if no actions are selected, its value should be □
        /// </summary>
        public void ActionUserControlCheckedChanged(ActionUserControl sender)
        {
            bool allChecked = true;
            bool allUnchecked = true;

            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                if (!auc.Selected)
                {
                    allChecked = false;
                }
                else if (auc.Selected)
                {
                    allUnchecked = false;
                }
                if (!allChecked && !allUnchecked)
                {
                    selectedAllCheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }

            if (allChecked)
                selectedAllCheckBox.CheckState = CheckState.Checked;



            else if (allUnchecked)
                selectedAllCheckBox.CheckState = CheckState.Unchecked;

            UpdateProcessButtons();
        }

        /// <summary>
        /// After a change of which actions should be processed (ActionUserControlEnableCheckedChanged),
        /// this method disables buttons for processing actions if there are none of them.
        /// </summary>
        private void UpdateProcessButtons()
        {
            if (WorkingState == State.Connected)
            {
                optimizeButton.Enabled = SomeActionsSelectedForProcessing();
                codeGenButton.Enabled = SomeActionsSelectedForProcessing();
                replayButton.Enabled = SomeActionsSelectedForProcessing();
            }
        }

        /// <summary>
        /// Change of processed actions.
        /// E.g. Enabled -> Selected (and vice versa)
        /// </summary>
        public void ActionUserControlEnableCheckedChanged(ActionUserControl sender)
        {
            UpdateProcessButtons();
        }

        /// <summary>
        /// UI logic for showing FilterForm on the "Filter" button click.
        /// </summary>
        private void filterButton_Click(object sender, EventArgs e)
        {
            filterForm = new FilterForm(this);
            if (activeFilter != null)
                filterForm.SetFilter(activeFilter);

            filterForm.Show();
        }

        /// <summary>
        /// Gets all the JSON data of actions with their respective AUC in a tuple.
        /// </summary>
        private Tuple<List<dynamic>, List<ActionUserControl>> GetRecordingActionsForOutput()
        {
            Tuple<List<dynamic>, List<ActionUserControl>> ret;
            List<dynamic> outputActions = new List<dynamic>();
            List<ActionUserControl> outputUserControl = new List<ActionUserControl>();

            if (edit.StartupHints != null)
                outputActions.Add(edit.StartupHints);

            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                if (enabledRadioButton.Checked)
                {
                    if (auc.EnabledForOutput)
                    {
                        outputActions.Add(auc.ExportActionForOutput());
                        outputUserControl.Add(auc);
                    }
                }
                else if (selectedRadioButton.Checked)
                {
                    if (auc.Selected)
                    {
                        outputActions.Add(auc.ExportActionForOutput());
                        outputUserControl.Add(auc);
                    }
                }

            }

            ret = new Tuple<List<dynamic>, List<ActionUserControl>>(outputActions, outputUserControl);
            return ret;
        }

        /// <summary>
        /// Gets all the actions that should be sent to the Backend for optimization.
        /// </summary>
        private List<Recording> GetRecordingsForOptimize()
        {
            List<Recording> outputActions = new List<Recording>();
            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                if (enabledRadioButton.Checked)
                {
                    if (auc.EnabledForOutput)
                        outputActions.Add(auc.ExportRecordingForSave());
                }
                else if (selectedRadioButton.Checked)
                {
                    if (auc.Selected)
                        outputActions.Add(auc.ExportRecordingForSave());
                }

            }

            return outputActions;
        }

        /// <summary>
        /// Sends the actions to the Backend that performs optimization (see Backend docs or text).
        /// After backend replies with optimized actions, this method updates UI respectively.
        /// </summary>
        private void optimizeButton_Click(object sender, EventArgs e)
        {
            if (actionsFlowLayoutPanel.Controls.Count == 0)
                return;

            string json = JsonConvert.SerializeObject(GetRecordingsForOptimize(), ConfigManager.JsonSettings);
            sock.SendFrame("optimize");
            sock.SendFrame(json);
            json = sock.ReceiveFrameString();
            List<Recording> actions = JsonConvert.DeserializeObject<List<Recording>>(json, ConfigManager.JsonSettings);
            edit.Recordings = actions;

            actionsFlowLayoutPanel.Controls.Clear();
            actions.ForEach(a => LoadRecording(a));
        }

        /// <summary>
        /// Change of processed actions.
        /// E.g. Enabled -> Selected (and vice versa)
        /// </summary>
        private void processRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProcessButtons();
        }

        /// <summary>
        /// Serializes actions with options for code generation and sends those to the Backend.
        /// After a reply from the Backend containing Puppeteer code, it shows it in a text editor.
        /// </summary>
        private void codeGenButton_Click(object sender, EventArgs e)
        {
            string actionsJson = JsonConvert.SerializeObject(GetRecordingActionsForOutput().Item1, ConfigManager.JsonSettings);
            string codeGenOptsJson = JsonConvert.SerializeObject(edit.Config.CodeGenConfig, ConfigManager.JsonSettings);
            sock.SendFrame("codeGen");
            sock.SendFrame(codeGenOptsJson);
            sock.SendFrame(actionsJson);
            string code = sock.ReceiveFrameString();
            CodeGenEditor cge = new CodeGenEditor();
            cge.SetEditorText(code);
            cge.Show();
        }

        /// <summary>
        /// Starts replaying: prepares UI and fires up the replayTask
        /// </summary>
        private void replayButton_Click(object sender, EventArgs e)
        {
            if (IsBrowserConnected())
            {
                cts = new CancellationTokenSource();
                cancelToken = cts.Token;
                replayViewForm?.Close();
                replayViewForm = new ReplayViewForm(this, cts);
                SetReplayEndedVisibility(false);
                replayViewForm.Show();

                replayTask = Task.Factory.StartNew(ReplayTask, cancelToken);
            }

        }

        /// <summary>
        /// Highlight the AUC given by the id, uses the given Color c.
        /// </summary>
        /// <param name="id">id of AUC to highlight</param>
        /// <param name="c">Color to use for highlighting</param>
        public void HighlightActionUserControlById(int id, Color c)
        {
            lock (colorChangeLck)
            {
                if (oldErrorHighlightedAuc != null)
                    UiSafeOperation(() =>
                    {
                        oldErrorHighlightedAuc.BackColor = default;
                        actionsFlowLayoutPanel.ScrollControlIntoView(runningNowHighlightedAuc);
                    });

                if (runningNowHighlightedAuc != null)
                {
                    UiSafeOperation(() =>
                    {
                        runningNowHighlightedAuc.BackColor = Color.DarkGoldenrod;
                        actionsFlowLayoutPanel.ScrollControlIntoView(runningNowHighlightedAuc);
                    });
                }

                foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
                {
                    if (auc.Id == id)
                    {
                        oldErrorHighlightedAuc = auc;
                        auc.BackColor = c;
                        actionsFlowLayoutPanel.ScrollControlIntoView(auc);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the highlighting of error highlighted AUC.
        /// </summary>
        public void ClearErrorCustomColors()
        {
            lock (colorChangeLck)
            {
                if (oldErrorHighlightedAuc != null)
                {
                    UiSafeOperation(() => oldErrorHighlightedAuc.BackColor = default);
                    oldErrorHighlightedAuc = null;
                    if (runningNowHighlightedAuc != null)
                    {
                        UiSafeOperation(() =>
                        {
                            runningNowHighlightedAuc.BackColor = Color.DarkGoldenrod;
                            actionsFlowLayoutPanel.ScrollControlIntoView(runningNowHighlightedAuc);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Sets the UI to match the state of either replaying or not replaying.
        /// </summary>
        private void SetReplayActiveUi(bool state)
        {
            UiSafeOperation(() =>
            {
                recordButton.Enabled = !state;
                optimizeButton.Enabled = !state;
                replayButton.Enabled = !state;
                codeGenButton.Enabled = !state;


                foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
                {
                    auc.Enabled = !state;
                }
            });
        }

        /// <summary>
        /// If an error occurs during replaying, this method adds the error to the error list.
        /// </summary>
        /// <param name="msg">Message to add to the error list</param>
        /// <param name="id">Matching AUC id</param>
        private void AddErrorToReplayViewForm(string msg, int id)
        {
            UiSafeOperation(() => replayViewForm.AddError(msg, id));
        }

        /// <summary>
        /// Sets the visibility of the message "Replay Ended"
        /// </summary>
        private void SetReplayEndedVisibility(bool visibility)
        {
            UiSafeOperation(() => replayViewForm.SetRecordingEnded(visibility));
        }

        /// <summary>
        /// Performs the given action that might be running on a different thread and updates UI safely.
        /// </summary>
        private void UiSafeOperation(Action a)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(() => a()));

            else
                a();
        }

        /// <summary>
        /// Clears highlighting of every action.
        /// </summary>
        public void ClearAucCustomColors()
        {
            lock (colorChangeLck)
            {
                foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
                {
                    UiSafeOperation(() => auc.BackColor = default);
                }
            }
        }

        /// <summary>
        /// Sends the message "finished" to the Backend, this message informs that there are no other actions to be replayed.
        /// Then this method waits for the confirmation that the Backend finished replaying the last action.
        /// Finally it changes UI that notifies about finished replaying.
        /// </summary>
        private void FinishReplay()
        {
            sock.SendFrame("finished");
            string msg = sock.ReceiveFrameString();
            while (msg != "evaluated")
            {
                msg = sock.ReceiveFrameString();
            }
            runningNowHighlightedAuc = null;
            UiSafeOperation(() => replayViewForm.ReplayEndedNow());
            SetReplayActiveUi(false);

        }

        /// <summary>
        /// Called when the focused action should be deleted.
        /// E.g. called after pressing Shift+Del.
        /// </summary>
        public void DeleteRequested()
        {
            List<ActionUserControl> toRemove = new List<ActionUserControl>();
            foreach (ActionUserControl auc in actionsFlowLayoutPanel.Controls)
            {
                if (auc.Selected)
                    toRemove.Add(auc);
            }
            toRemove.ForEach(auc => actionsFlowLayoutPanel.Controls.Remove(auc));
            selectedAllCheckBox.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// Highlight the given AUC, uses the given Color c.
        /// </summary>
        /// <param name="id">id of AUC to highlight</param>
        /// <param name="c">Color to use for highlighting</param>
        private void AucColorChangeSafe(ActionUserControl auc, Color c)
        {
            lock (colorChangeLck)
            {
                UiSafeOperation(() =>
                {
                    auc.BackColor = c;
                    actionsFlowLayoutPanel.ScrollControlIntoView(auc);
                });
                runningNowHighlightedAuc = auc;
            }
        }

        /// <summary>
        /// This method sends actions to Backend to have them replayed.
        /// Execution of this method is started by replayTask.
        /// </summary>
        private void ReplayTask()
        {
            SetReplayActiveUi(true);

            Tuple<List<dynamic>, List<ActionUserControl>> t = GetRecordingActionsForOutput();
            List<dynamic> actions = t.Item1;
            List<ActionUserControl> aucs = t.Item2;

            string codeGenConfig = JsonConvert.SerializeObject(edit.Config.PlayerOptions, ConfigManager.JsonSettings);
            string actionsJson = JsonConvert.SerializeObject(actions, ConfigManager.JsonSettings);
            sock.SendFrame("replay");
            sock.SendFrame(codeGenConfig);
            sock.SendFrame(actionsJson);

            actions.RemoveAt(0); //startupHints

            for (int i = 0; i < actions.Count; ++i)
            {

                if (cts.IsCancellationRequested)
                {
                    if (!forceCancelReplay)
                        FinishReplay();
                    return;
                }

                lock (colorChangeLck)
                {
                    runningNowHighlightedAuc = aucs[i];
                }
                AucColorChangeSafe(aucs[i], Color.DarkGoldenrod);

                sock.SendFrame(i.ToString());
                bool b;
                string m = null;
                b = sock.ReceiveFrameStringTimeout(out m, 100);
                while (!b && !cts.IsCancellationRequested)
                    b = sock.ReceiveFrameStringTimeout(out m, 100);


                if (cts.IsCancellationRequested)
                {
                    if (!forceCancelReplay)
                    {
                        FinishReplay();
                        AucColorChangeSafe(aucs[i], default);
                    }

                    return;
                }

                while (m != "evaluated")
                {
                    //exception occurred
                    if (m != "true")
                        AddErrorToReplayViewForm(m, aucs[i].Id);

                    b = false;
                    while (!b && !cts.IsCancellationRequested)
                        b = sock.ReceiveFrameStringTimeout(out m, 100);


                    if (cts.IsCancellationRequested)
                    {
                        if (!forceCancelReplay)
                        {
                            FinishReplay();
                            AucColorChangeSafe(aucs[i], default);
                        }
                        return;
                    }


                }

                AucColorChangeSafe(aucs[i], default);
            }

            runningNowHighlightedAuc = null;
            sock.SendFrame("finished");

            SetReplayEndedVisibility(true);
            SetReplayActiveUi(false);
        }

    }
}
