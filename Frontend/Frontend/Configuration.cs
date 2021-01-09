namespace Frontend
{
    /// <summary>
    /// Represents the whole configuration of the Frontend application.
    /// </summary>
    public class Configuration
    {
        public PuppeteerOptions PuppeteerConfig { get; set; }
        public CodeGeneratorOptions CodeGeneratorConfig { get; set; }
        public PlayerOptions PlayerOptions { get; set; }
        public NodeJsOptions NodeJsOptions { get; set; }
        public RecordedEvents RecordedEvents { get; set; }
    }
}
