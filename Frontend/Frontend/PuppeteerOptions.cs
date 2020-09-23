using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Frontend
{
    public class PuppeteerOptions
    {
        [JsonProperty(PropertyName = "headless")] 
        public bool Headless { get; set; } = false;

        [JsonProperty(PropertyName = "defaultViewport")]
        public Viewport Viewport { get; set; }

        [JsonProperty(PropertyName = "devtools")]
        public bool DevTools { get; set; }

        [JsonProperty(PropertyName = "slowMo")]
        public int SlowMo { get; set; }
    }

    class ConnectPuppeteerOptions : PuppeteerOptions
    {
        [JsonProperty(PropertyName = "browserURL")]
        public Uri EndPoint { get; set; }
    }

    class LaunchPuppeteerOptions : PuppeteerOptions
    {
        [JsonProperty(PropertyName = "executablePath", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string ExecutablePath { get; set; }
    }

    public class Viewport
    {
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "deviceScaleFactor")]
        public double DeviceScaleFactor { get; set; }

        [JsonProperty(PropertyName = "isMobile")]
        public bool IsMobile { get; set; }

        [JsonProperty(PropertyName = "hasTouch")]
        public bool HasTouch { get; set; }

        [JsonProperty(PropertyName = "isLandscape")]
        public bool IsLandscape { get; set; }
    }
}
