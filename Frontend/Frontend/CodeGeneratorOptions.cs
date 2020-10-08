using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Frontend
{
    public class CodeGeneratorOptions
    {
        [JsonProperty(PropertyName = "addWaitFor")]
        public bool AddWaitFor { get; set; } = true;

        [JsonProperty(PropertyName = "waitForNavigation")]
        public bool WaitForNavigation { get; set; } = true;

        [JsonProperty(PropertyName = "blankLinesBetweenCodeBlocks")]
        public bool BlankLinesBetweenCodeBlocks { get; set; } = true;


        [JsonProperty(PropertyName = "addRequirePuppeteer")]
        public bool AddRequirePuppeteer { get; set; } = true;

        [JsonProperty(PropertyName = "browserClose")]
        public bool BrowserClose { get; set; } = true;



        [JsonProperty(PropertyName = "browserDisconnect")]
        [Browsable(false)]
        private bool browserDisconnect = false;


        [JsonProperty(PropertyName = "browserConnect")]
        [Browsable(false)]
        private bool browserConnect = false;


        [JsonProperty(PropertyName = "addLaunchOrConnect")]
        [Browsable(false)]
        private bool addLaunchOrConnect = true;

        [JsonProperty(PropertyName = "browserLaunch")]
        [Browsable(false)]
        private bool browserLaunch = true;




        [JsonProperty(PropertyName = "waitForNavigationOptions")]
        [Browsable(false)]
        public string WaitForNavigationOptionJson => "{ \"waitUntil\": \"" + Enum.GetName(typeof(WaitForNavigation), WaitForNavigationOptions) + "\" }";


        public WaitForNavigation WaitForNavigationOptions { get; set; }

        [JsonProperty(PropertyName = "waitForTargetOptions")]
        [Browsable(false)]
        public string WaitForTargetOptions => "{ \"timeout\": "+ WaitForTargetTimeoutMs +" }";

        [JsonIgnore] 
        public int WaitForTargetTimeoutMs { get; set; } = 5000;

        [JsonProperty(PropertyName = "catchErrors")]
        public bool CatchErrors { get; set; } = false;

        [JsonProperty(PropertyName = "logErrors")]
        public bool LogErrors { get; set; } = false;

        [JsonProperty(PropertyName = "typeOptions")]
        [Browsable(false)]
        public string TypeOptions => "{ \"delay\": " + KeystrokeDelayMs +" }";

        [JsonIgnore]
        public int KeystrokeDelayMs { get; set; } = 100;

        [JsonProperty(PropertyName = "indent")]
        public int Indent { get; set; } = 3;

        [JsonProperty(PropertyName = "addXpathFunctions")]
        public bool AddXpathFunctions { get; set; } = false;

    }
}
