namespace Frontend
{
    /// <summary>
    /// See https://github.com/puppeteer/puppeteer/blob/v5.5.0/docs/api.md#pagewaitfornavigationoptions for more information about available types.
    /// </summary>
    public enum WaitForNavigation
    {
        networkidle0, networkidle2, domcontentloaded, load
    }
}
