using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace KomicDownloader.Services
{
    public class ParseService : IParserService
    {
        public IDocument DocumentCached { get; private set; }
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
            if (DocumentCached == null)
                DocumentCached = await browsingContext.OpenAsync(url, cancellationToken);

            return DocumentCached;
        }
    }
}
