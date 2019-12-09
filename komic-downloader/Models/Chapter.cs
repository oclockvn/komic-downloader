using System.Collections.Generic;

namespace KomicDownloader.Models
{
    public class Chapter
    {
        public string Url { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Name { get; set; }
    }
}
