namespace Frontend
{
    /// <summary>
    /// One action with Id and current ui configuration
    /// </summary>
    public class Recording
    {
        public int Id { get; set; }
        public dynamic Action { get; set; }
        public UiConfig UiConfig { get; set; }
    }

    public class UiConfig
    {
        public Target Target { get; set; } = Target.Locator;
        public int SelectedLocatorIndex { get; set; } = 0;
        public bool Enabled { get; set; } = true;
        public bool Selected { get; set; } = false;
    }
    public enum Target
    {
        Locator, Selector
    }
}
