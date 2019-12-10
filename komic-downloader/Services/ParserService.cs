using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KomicDownloader.Services
{
    public class ParseService : IParserService
    {
        private IDictionary<string, IDocument> caches = new Dictionary<string, IDocument>();
        private readonly IBrowsingContext browsingContext;

        public ParseService(IBrowsingContext browsingContext)
        {
            this.browsingContext = browsingContext;
        }

        public async Task<List<T>> ParseAsync<T>(string url, string selector, Func<IElement, T> selection, CancellationToken cancellationToken = default)
        {
            var document = await BrowseAsync(url, cancellationToken);

            if (document == null)
                throw new Exception("Page could not load");

            var elements = document.QuerySelectorAll(selector);
            if (elements?.Length == 0)
                throw new Exception("Invalid selector");

            return elements.Select(selection).ToList();
        }

        private async Task<IDocument> BrowseAsync(string url, CancellationToken cancellationToken)
        {
            IDocument document;
            if (caches.ContainsKey(url))
            {
                document = caches[url];
            }
            else
            {
                document = await browsingContext.OpenAsync(url, cancellationToken);

                // TODO: remove idle documents
                if (caches.Count > 100)
                {
                    Console.WriteLine("Cache reaches maximum limit, resetting...");
                    caches = new Dictionary<string, IDocument>();
                }
                else
                {
                    caches.Add(url, document);
                }
            }

            return document;
        }
    }
}
