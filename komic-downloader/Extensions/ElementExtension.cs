using AngleSharp.Dom;
using System;

namespace KomicDownloader.Extensions
{
    public static class ElementExtension
    {
        public static string GetAbsuluteUrl(this IElement element, string host)
        {
            if (element == null)
                return string.Empty;

            var href = element.GetAttribute("href");

            if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
                return href;

            return $"{host.TrimEnd('/')}/{href.TrimStart('/')}";
        }

        public static string GetTitleOrContent(this IElement element)
        {
            if (element == null)
                throw new InvalidOperationException();

            var title = element.GetAttribute("title");
            if (!string.IsNullOrWhiteSpace(title))
                return title;

            return element.TextContent;
        }

        /// <summary>
        /// get the left part of uri
        /// </summary>
        /// <param name="url">from this https://example.com/foo</param>
        /// <returns>return this https://example.com</returns>
        public static string GetAbsoluteHost(this string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return string.Empty;

            var uri = new Uri(url);
            return uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        }
    }
}
