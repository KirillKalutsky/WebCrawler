using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class PageArchitectureSite : ICrawlableSource
    {
        public string StartUrl { get; set; }
        public string EndUrl { get; set; }
        public string LinkURL { get; set; }
        public HtmlElement LinkElement { get; set; }
        public Dictionary<string, string> ParseEventProperties { get; set; }

        private string lastEventLink;
        private int? maxCountEvents;


        string currentEventLink;
        int currentSeanceCrawledEventCount;

        /*public override async IAsyncEnumerable<Event> CrawlAsync()
        {
            isRun = true;
            currentSeanceCrawledEventCount = 0;
            var pageCounter = 1;
            var url = $"{StartUrl}{pageCounter}{EndUrl}";
            while (isRun)
            {
                var page = await PageLoader.LoadPageAsync(url);
                if (!page.Item1.IsSuccessStatusCode)
                    yield break;

                var pages = (await PageLoader.GetPageElementAsync(url, LinkElement))
                    .Select(x => PageLoader.LoadPageAsync($"{LinkURL}{x}")).ToList();

                while (pages.Any())
                {
                    var tP = await Task.WhenAny(pages);
                    pages.Remove(tP);
                    var p = await tP;

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(await p.Item1.Content.ReadAsStringAsync());

                    var news = NewsParser.ParseHtmlPage(document, ParseEventProperties);

                    news.Link = p.Item2;
                    currentEventLink = page.Item2;
                    currentSeanceCrawledEventCount+=1;
                    isRun = StopCrawl();
                    yield return news;
                }

                pageCounter+=1;
                url = $"{StartUrl}{pageCounter}{EndUrl}";
            }
        }*/

        /*public override bool StopCrawl()
        {
            if (LastEvent != null)
                return LastEvent.Link.Equals(currentEventLink);

            return currentSeanceCrawledEventCount < 1000;
        }*/

     
        

       

        public override async IAsyncEnumerable<Event> CrawlAsync(string lastLink, int? maxCountEvents)
        {
            lastEventLink = lastLink;
            this.maxCountEvents = maxCountEvents;
            isCrawl = true;
            currentSeanceCrawledEventCount = 0;
            var pageCounter = 1;
            var url = $"{StartUrl}{pageCounter}{EndUrl}";
            while (isCrawl)
            {
                var page = await PageLoader.LoadPageAsync(url);
                if (!page.Item1.IsSuccessStatusCode)
                    yield break;

                var pages = (await PageLoader.GetPageElementAsync(url, LinkElement))
                    .Select(x => PageLoader.LoadPageAsync($"{LinkURL}{x}")).ToList();

                while (pages.Any())
                {
                    var tP = await Task.WhenAny(pages);
                    pages.Remove(tP);
                    var p = await tP;

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(await p.Item1.Content.ReadAsStringAsync());

                    var news = NewsParser.ParseHtmlPage(document, ParseEventProperties);

                    news.Link = p.Item2;
                    currentEventLink = page.Item2;
                    currentSeanceCrawledEventCount += 1;
                    isCrawl = StopCrawl();
                    yield return news;
                }

                pageCounter += 1;
                url = $"{StartUrl}{pageCounter}{EndUrl}";
            }
        }

        public override bool StopCrawl()
        {
            if (lastEventLink != null)
                return currentEventLink != lastEventLink;
            if (maxCountEvents != null)
                return currentSeanceCrawledEventCount < maxCountEvents;
            return true;
        }
    }
}
