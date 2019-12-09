using System.IO;
using System.Threading.Tasks;

namespace KomicDownloader.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Store input stream into path
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task StoreAsync(Stream input, string filename, string path);

        /// <summary>
        /// Store a url into path
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task StoreAsync(string url, string filename, string path);
    }
}
