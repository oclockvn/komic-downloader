using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace KomicDownloader.Services
{
    public class StorageService : IStorageService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public Task StoreAsync(Stream input, string filename, string path)
        {
            throw new System.NotImplementedException();
        }

        public async Task StoreAsync(string url, string filename, string path)
        {
            var fullpath = Path.Combine(path, filename);
            if (File.Exists(fullpath))
                return;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                // TODO: handle server prevents download
                using (var stream = await httpClient.GetStreamAsync(url))
                {
                    using (var file = new FileStream(fullpath, FileMode.Create))
                    {
                        await stream.CopyToAsync(file);
                    }
                }

                Debug.WriteLine($"{url} saved to {fullpath} success");
                Console.WriteLine($"{filename} has been saved successful");
            }
            catch (HttpRequestException e)
            {
                // ignore it
                Debug.WriteLine($"Download file ${url} got an error: {e.Message}");
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Save file {url} to {fullpath} got an error: {e.Message}");
            }
        }
    }
}
