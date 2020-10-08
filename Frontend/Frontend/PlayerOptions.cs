using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Frontend
{
    public class PlayerOptions
    {
        //[JsonIgnore]
        //public bool BrowserCloseOrDisconnect
        //{
        //    get
        //    {
        //        if (PuppeteerOptions is ConnectPuppeteerOptions)
        //            return browserDisconnect;

        //        return browserClose;
        //    }
        //    set
        //    {
        //        if (PuppeteerOptions is ConnectPuppeteerOptions)
        //        {
        //            browserDisconnect = value;
        //            browserClose = false;
        //        }
        //        else
        //        {
        //            browserClose = value;
        //            browserDisconnect = false;
        //        }
        //    }
        //}


        [Browsable(false)]
        [JsonProperty(PropertyName = "browserDisconnect")]
        private bool browserDisconnect = false;

        [Browsable(false)]
        [JsonProperty(PropertyName = "browserClose")]
        private bool browserClose = false;

        //[Browsable(false)]
        //[JsonProperty(PropertyName = "browserConnect")]
        //private bool browserConnect = false;

        //[Browsable(false)]
        //[JsonProperty(PropertyName = "browserLaunch")]
        //private bool browserLaunch = true;

        [JsonProperty(PropertyName = "waitForNavigationOptions")]
        [Browsable(false)]
        public string WaitForNavigationOptionJson => "{ \"waitUntil\": \"" + Enum.GetName(typeof(WaitForNavigation), WaitForNavigationOptions) + "\" }";


        public WaitForNavigation WaitForNavigationOptions { get; set; }

        [JsonProperty(PropertyName = "waitForTargetOptions")]
        [Browsable(false)]
        public string WaitForTargetOptions => "{ \"timeout\": " + WaitForTargetTimeoutMs + " }";

        [JsonIgnore]
        public int WaitForTargetTimeoutMs { get; set; } = 5000;

        [Browsable(false)]
        [JsonProperty(PropertyName = "catchErrors")]
        public bool CatchErrors { get; set; } = true;

        [Browsable(false)]
        [JsonProperty(PropertyName = "logErrors")]
        public bool LogErrors { get; set; } = true;

        [JsonProperty(PropertyName = "sendErrorsBack")]
        public bool SendErrorsBack = true;

        [JsonProperty(PropertyName = "typeOptions")]
        [Browsable(false)]
        public string TypeOptions => "{ \"delay\": " + KeystrokeDelayMs + " }";

        [JsonIgnore]
        public int KeystrokeDelayMs { get; set; } = 100;

        [Browsable(false)]
        [JsonProperty(PropertyName = "evaluationFinishedAck")]
        public bool EvaluationFinishedAck { get; set; } = false;
        //  [Browsable(false)] 
        //  public PuppeteerOptions PuppeteerOptions { get; set; } = null;
    }
}
