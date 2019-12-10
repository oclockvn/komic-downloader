using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using KomicDownloader.Extensions;
using KomicDownloader.Services;
using Microsoft.Extensions.Configuration;

namespace KomicDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Comic downloader v1.0 credit to oclockvn");
            var url = string.Empty;

            var comicConfig = new ComicConfig();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("sites.json", true, true)
                .Build();

            config.GetSection("Comic").Bind(comicConfig);

            var chapterSelector = config.GetSection("ChapterSelector").Value;
            var nameSelector = config.GetSection("NameSelector").Value;
            var imageSelector = config.GetSection("ImageSelector").Value;

            var dir = AppDomain.CurrentDomain.BaseDirectory;

            var browserConfig = AngleSharp.Configuration.Default.WithDefaultLoader();
            var browserContext = AngleSharp.BrowsingContext.New(browserConfig);
            var comicService = new ComicService(new ParseService(browserContext), new StorageService());

            do
            {
                Console.Write("Enter comic url or q to exit: ");
                url = Console.ReadLine();

                if (url.IsGoodUrl())
                {
                    var host = url.GetAbsoluteHost();
                    var siteConfig = comicConfig.Sites.First(x => x.Host == host);
                    await comicService.DownloadAsync(url, siteConfig.NameSelector, siteConfig.ChapterSelector, siteConfig.ImageSelector, dir);

                    Console.Write("Continue (y/n): ");
                    var cont = Console.ReadLine();

                    if (cont == "n")
                    {
                        break;
                    }
                }
            }
            while (url != "q");

            Console.Write("Press any key to close...");
        }
    }
}
