using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var crawler = new Crawler();
            var sources = new List<Tuple<CrawlableSource, string, int?>>()
            {
                Tuple.Create<CrawlableSource, string, int?>
                (
                    new PageArchitectureSite()
                            {
                                StartUrl = "https://veved.ru/eburg/news/page/",
                                LinkURL = "",
                                EndUrl="",
                                LinkElement = new HtmlElement
                                {
                                    XPath = @".//a[@class='box']",
                                    AttributeName = "href"
                                },

                                ParseEventProperties = new Dictionary<string, string>
                                {
                                    { "Body", ".//div[@class='fullstory-column']" },
                                    //{ "Date", ".//div[@class='vremya']" },
                                    { "Title", ".//h1[@class='zagolovok1']" }
                                }
                            },
                    null,
                    100
                ),

            };
            var events = crawler.StartAsync(sources);
            var counter = 1;
            await foreach (var e in events)
            {
                Console.WriteLine(counter);
                Console.WriteLine(e.Link);
                counter++;
            }
        }
    }
}
