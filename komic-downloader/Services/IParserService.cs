using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace KomicDownloader.Services
{
    public interface IParserService
    {
        /// <summary>
        /// Download data from url then parse using selector into selection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="selector"></param>
        /// <param name="selection"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> ParseAsync<T>(string url, string selector, Func<IElement, T> selection, CancellationToken cancellationToken = default);
    }
}
