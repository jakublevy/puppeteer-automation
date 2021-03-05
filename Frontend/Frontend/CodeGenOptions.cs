using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Frontend
{
    //Disables (in this case the useless) "unused property warning"
#pragma warning disable CS0414

    /// <summary>
    /// Options that change the behaviour of a code generator.
    /// </summary>
    public class CodeGenOptions
    {
        [JsonProperty(PropertyName = "addWaitFor")]
        [Description("Adds waitForSelector or waitForXPath before interacting with an element.")]
        public bool AddWaitFor { get; set; } = true;

        [JsonProperty(PropertyName = "waitForNavigation")]
        [Description("Defines whether the script should wait for a navigation.")]
        public bool WaitForNavigation { get; set; } = true;

        [JsonProperty(PropertyName = "blankLinesBetweenCodeBlocks")]
        [Description("Adds a blank line between interactions with different elements.")]
        public bool BlankLinesBetweenCodeBlocks { get; set; } = true;


        [JsonProperty(PropertyName = "addRequirePuppeteer")]
        [Description("Adds Puppeteer import statement\ne.g. const puppeteer = require('puppeteer')")]
        public bool AddRequirePuppeteer { get; set; } = true;

        [JsonProperty(PropertyName = "browserClose")]
        [Description("Adds browser.close() closing statement at the end of the script.")]
        public bool BrowserClose { get; set; } = true;



        [JsonProperty(PropertyName = "browserDisconnect")]
        [Browsable(false)]
        private readonly bool browserDisconnect = false;


        [JsonProperty(PropertyName = "browserConnect")]
        [Browsable(false)]
        private readonly bool browserConnect = false;


        [JsonProperty(PropertyName = "addLaunchOrConnect")]
        [Browsable(false)]
        private readonly bool addLaunchOrConnect = true;

        [JsonProperty(PropertyName = "browserLaunch")]
        [Browsable(false)]
        private readonly bool browserLaunch = true;




        [JsonProperty(PropertyName = "waitForNavigationOptions")]
        [Browsable(false)]
        public string WaitForNavigationOptionJson => "{ \"waitUntil\": \"" +
                                                     Enum.GetName(typeof(WaitForNavigation), WaitForNavigationOptions) +
                                                     "\" }";


        [Description("Defines a condition until the script should wait after performing a navigation.")]
        public WaitForNavigation WaitForNavigationOptions { get; set; }

        [JsonProperty(PropertyName = "waitForTargetOptions")]
        [Browsable(false)]
        public string WaitForTargetOptions
        {
            get => "{ \"timeout\": " + WaitForTargetTimeoutMs + " }";
            set => WaitForTargetTimeoutMs = JsonConvert.DeserializeObject<dynamic>(value).timeout;
        }

        [JsonIgnore]
        [Description("Time that should be waited for elements in waitForSelector and waitForXPath statements.")]
        public int WaitForTargetTimeoutMs { get; set; } = 5000;

        [JsonProperty(PropertyName = "catchErrors")]
        [Description("Adds .catch() after each statement.")]
        public bool CatchErrors { get; set; } = false;

        [JsonProperty(PropertyName = "logErrors")]
        [Description("Adds .catch() body reporting errors.")]
        public bool LogErrors { get; set; } = false;

        [JsonProperty(PropertyName = "typeOptions")]
        [Browsable(false)]
        public string TypeOptions
        {
            get => "{ \"delay\": " + KeystrokeDelayMs + " }";
            set => KeystrokeDelayMs = JsonConvert.DeserializeObject<dynamic>(value).delay;
        }

        [JsonIgnore]
        [Description("Sets a pause (ms) between each keystroke.")]
        public int KeystrokeDelayMs { get; set; } = 100;

        [JsonProperty(PropertyName = "indent")]
        [Description("Sets the number of spaces for indentation.")]
        public int Indent { get; set; } = 3;

        [JsonProperty(PropertyName = "addXpathFunctions")]
        [Description("Adds Locators --> XPath conversion and element retrieving methods to the preamble.")]
        public bool AddXpathFunctions { get; set; } = false;

    }
}
