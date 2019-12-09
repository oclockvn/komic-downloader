using System.Threading.Tasks;

namespace KomicDownloader.Services
{
    public interface IComicService
    {
        /// <summary>
        /// Download comic
        /// </summary>
        /// <param name="comicUrl"></param>
        /// <param name="nameSelector"></param>
        /// <param name="chapterSelector"></param>
        /// <param name="imageSelector"></param>
        /// <param name="storeDirectory"></param>
        /// <returns></returns>
        Task DownloadAsync(string comicUrl, string nameSelector, string chapterSelector, string imageSelector, string storeDirectory);
    }
}
