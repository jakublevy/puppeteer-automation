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
        public string WaitForNavigationOptions { get; set; } = "{ \"waitUntil\": \"networkidle0\" }";

        [JsonProperty(PropertyName = "waitForTargetOptions")]
        public string WaitForTargetOptions { get; set; } = "{ \"timeout\": 10000 }";

        [Browsable(false)]
        [JsonProperty(PropertyName = "catchErrors")]
        public bool CatchErrors { get; set; } = true;

        [Browsable(false)]
        [JsonProperty(PropertyName = "logErrors")]
        public bool LogErrors { get; set; } = true;

        [JsonProperty(PropertyName = "sendErrorsBack")]
        public bool SendErrorsBack = true;

        [JsonProperty(PropertyName = "typeOptions")]
        public string TypeOptions { get; set; } = "{ \"delay\": 100 }";

        [Browsable(false)]
        [JsonProperty(PropertyName = "evaluationFinishedAck")]
        public bool EvaluationFinishedAck { get; set; } = false;
        //  [Browsable(false)] 
        //  public PuppeteerOptions PuppeteerOptions { get; set; } = null;
    }
}
