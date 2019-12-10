namespace KomicDownloader
{
    public class ComicConfig
    {
        public SiteConfig[] Sites { get; set; }
    }

    public class SiteConfig
    {
        public string Host { get; set; }
        public string NameSelector { get; set; }
        public string ChapterSelector { get; set; }
        public string ImageSelector { get; set; }
    }
}
