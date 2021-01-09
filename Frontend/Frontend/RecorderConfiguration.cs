namespace Frontend
{
    public class RecorderConfiguration
    {
        /// <summary>
        /// Puppeteer init/startup configuration.
        /// It is used in backend in puppeteer.launch(PuppeteerOptions) or puppeteer.connect(PuppeteerOptions).
        /// </summary>
        public PuppeteerOptions PuppeteerOptions { get; set; }

        /// <summary>
        /// Events that are captured.
        /// </summary>
        public RecordedEvents RecordedEvents { get; set; }
    }
}
