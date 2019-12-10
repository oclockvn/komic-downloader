using System;
using System.Threading.Tasks;
using KomicDownloader.Services;
using KomicDownloader.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using AngleSharp;

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

            var chapterSelector = ".comic-description > a";// config.GetValue<string>("ChapterSelector");
            var nameSelector = ".comic-info .info > h1.name";// config.GetValue<string>("NameSelector");
            var imageSelector = ".chapter-content p > img";// config.GetValue<string>("ImageSelector");

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
