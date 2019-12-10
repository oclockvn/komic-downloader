using AngleSharp;
using KomicDownloader.Extensions;
using KomicDownloader.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace KomicDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Comic downloader v1.0 credit to oclockvn");
            var url = string.Empty;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            //var chapterSelector = ".comic-description > a";// config.GetValue<string>("ChapterSelector");
            //var nameSelector = ".comic-info .info > h1.name";// config.GetValue<string>("NameSelector");
            //var imageSelector = ".chapter-content p > img";// config.GetValue<string>("ImageSelector");

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
                    await comicService.DownloadAsync(url, nameSelector, chapterSelector, imageSelector, dir);

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
