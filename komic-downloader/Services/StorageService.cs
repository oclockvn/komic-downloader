using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace KomicDownloader.Services
{
    public class StorageService : IStorageService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public StorageService()
        {
        }

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

            using (var stream = await httpClient.GetStreamAsync(url))
            {
                using (var file = new FileStream(fullpath, FileMode.Create))
                {
                    await stream.CopyToAsync(file);
                }
            }
        }
    }
}
