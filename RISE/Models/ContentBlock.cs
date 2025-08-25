namespace RISE.Models
{
    public class ContentBlock
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;     // es. "Home.Intro", "Rules.DownloadText"
        public string Value { get; set; } = string.Empty;   // testo HTML/Markdown
    }
}
