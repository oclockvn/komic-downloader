using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KomicDownloader.Extensions;
using KomicDownloader.Models;

namespace KomicDownloader.Services
{
    public class ComicService : IComicService
    {
        private readonly IParserService parserService;
        private readonly IStorageService storageService;

        public ComicService(IParserService parserService, IStorageService storageService)
        {
            this.parserService = parserService;
            this.storageService = storageService;
        }

        private string CreateDirectoryIfNotExist(params string[] paths)
        {
            var fullpath = Path.Combine(paths);

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            return fullpath;
        }

        public async Task DownloadAsync(string comicUrl, string nameSelector, string chapterSelector, string imageSelector, string storeDirectory)
        {
            var host = comicUrl.GetAbsoluteHost();
            var names = await parserService.ParseAsync(comicUrl, nameSelector, x => x.GetTitleOrContent());
            var comicName = names.First().ToFriendlyUrl(500);

            Console.WriteLine($"Downloading commic {comicName} at {comicUrl}...");

            var storePath = CreateDirectoryIfNotExist(storeDirectory, comicName);

            var chapters = await parserService.ParseAsync(comicUrl, chapterSelector, x => new Chapter
            {
                Url = x.GetAbsuluteUrl(host),
                Name = x.GetTitleOrContent(),
                Images = new List<string>()
            }, default);

            if (chapters.Count == 0)
            {
                Console.WriteLine("No chapters found.");
                return;
            }

            Console.WriteLine($"{chapters.Count} chapter(s) found. Starting download...");

            var chapterIndex = 0;
            var chapterName = string.Empty;

            foreach (var chapter in chapters)
            {
                var images = await parserService.ParseAsync(chapter.Url, imageSelector, x => x.GetAttribute("src"), default);
                chapterName = chapter.Name.ToFriendlyUrl(500);

                storePath = CreateDirectoryIfNotExist(storePath, chapterName);

                Console.WriteLine($"Downloading chapter {chapter.Name}...{images.Count} image(s) found!");

                var index = 0;
                chapterIndex++;

                foreach (var image in images)
                {
                    // save image to {Comic}/{Chap-1}/{comic}-{chap}-1-1.png
                    var filename = $"{comicName}-{chapterName}-{chapterIndex}-{index++}.png";
                    var fullpath = Path.Combine(storePath, filename);

                    await storageService.StoreAsync(image, filename, fullpath);
                    Console.WriteLine($"{fullpath} has been saved successful");
                }
            }
        }
    }
}
