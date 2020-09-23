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



        [JsonProperty(PropertyName = "browserClose")]
        [Browsable(false)]
        private bool browserClose = false;

        [JsonProperty(PropertyName = "browserDisconnect")]
        [Browsable(false)]
        private bool browserDisconnect = false;

        [JsonProperty(PropertyName = "addRequirePuppeteer")]
        [Browsable(false)]
        private bool addRequirePuppeteer = false;

        [JsonProperty(PropertyName = "browserConnect")]
        [Browsable(false)]
        private bool browserConnect = false;

        [JsonProperty(PropertyName = "browserLaunch")]
        [Browsable(false)]
        private bool browserLaunch = false;



        [JsonProperty(PropertyName = "waitForNavigationOptions")]
        public string WaitForNavigationOptions { get; set; } = "{ \"waitUntil\": \"networkidle0\" }";

        [JsonProperty(PropertyName = "catchErrors")]
        public bool CatchErrors { get; set; } = false;

        [JsonProperty(PropertyName = "logErrors")]
        public bool LogErrors { get; set; } = false;

        [JsonProperty(PropertyName = "typeOptions")]
        public string TypeOptions { get; set; } = "{ \"delay\": 100 }";

        [JsonProperty(PropertyName = "indent")]
        public int Indent { get; set; } = 3;

        [JsonProperty(PropertyName = "addXpathFunctions")]
        public bool AddXpathFunctions { get; set; } = false;

    }
}
