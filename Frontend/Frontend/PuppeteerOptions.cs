using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Frontend
{
    /// <summary>
    /// Corresponds to a subset of Puppeteer.launch/connect,
    /// for more information consult: https://github.com/puppeteer/puppeteer/blob/v5.5.0/docs/api.md#puppeteerlaunchoptions 
    /// </summary>
    public class PuppeteerOptions
    {
        [JsonProperty(PropertyName = "headless")]
        public bool Headless { get; set; } = false;

        [JsonProperty(PropertyName = "defaultViewport", NullValueHandling = NullValueHandling.Include)]
        public Viewport Viewport { get; set; }

        [JsonProperty(PropertyName = "devtools")]
        public bool DevTools { get; set; }

        [JsonProperty(PropertyName = "slowMo")]
        public decimal SlowMo { get; set; }
    }

    internal class ConnectPuppeteerOptions : PuppeteerOptions
    {
        [JsonProperty(PropertyName = "browserURL")]
        public Uri EndPoint { get; set; }
    }

    internal class LaunchPuppeteerOptions : PuppeteerOptions
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
