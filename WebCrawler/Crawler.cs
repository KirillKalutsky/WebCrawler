using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Crawler
    {
        /*private CrawlableSource GetCrawlableSourceFromSource(Source s)
        {
            CrawlableSource crawbleSource;
            switch (s.SourceType)
            {
                case SourceType.PageSite:
                    var pageSite = JsonConvert.DeserializeObject<PageArchitectureSite>(s.Fields.Properties);
                    if (s.Events != null)
                        pageSite.LastEvent = s.Events.OrderBy(x => x.DateOfDownload).LastOrDefault();
                    crawbleSource = pageSite;
                    break;
                default:
                    var site = JsonConvert.DeserializeObject<PageArchitectureSite>(s.Fields.Properties);
                    site.LastEvent = s.Events.OrderBy(x => x.DateOfDownload).LastOrDefault();
                    crawbleSource = site;
                    break;
            }
            return crawbleSource;
        }*/

       /* Tuple<List<CrawlableSource>, Dictionary<IAsyncEnumerator<Event>, CrawlableSource>> GetCrawbleSourceEnumerator(List<Source> sourcers)
        {
            var fromCrawbleSourceToEnumerator = new Dictionary<IAsyncEnumerator<Event>, CrawlableSource>();
            var listSources = new List<CrawlableSource>();

            sourcers.ForEach(s =>
            {
                var source = GetCrawlableSourceFromSource(s);
                listSources.Add(source);
                var enumerator = source.CrawlAsync(s).GetAsyncEnumerator();
                fromCrawbleSourceToEnumerator[enumerator] = source;
            });

            return Tuple.Create(listSources, fromCrawbleSourceToEnumerator);
        }*/

        public async IAsyncEnumerable<Event> StartAsync(IEnumerable<Tuple<ICrawlableSource,string,int?>> sourcers)
        {
            foreach (var e in sourcers)
            {
                await foreach (var ev in e.Item1.CrawlAsync(e.Item2, e.Item3))
                    yield return ev;
            }
            /*var s = GetCrawbleSourceEnumerator(sourcers.ToList());
            var sourceEnumerators = s.Item2;

            var dictionary = new Dictionary<Task<bool>, IAsyncEnumerator<Event>>();


            while (sourceEnumerators.Any())
            {
                foreach (var en in sourceEnumerators.Keys)
                {
                    var next = en.MoveNextAsync().AsTask();
                    var flag = await next;
                    if (flag)
                    {
                        var news = en.Current;
                        yield return news;
                    }
                    else
                    {
                        sourceEnumerators.Remove(en);
                    }
                    //dictionary[next] = en;
                    //dictionary.Add(next,en);
                }
                *//*while (dictionary.Any())
                {
                    var cur = await Task.WhenAny(dictionary.Keys);
                    IAsyncEnumerator<Event> enumer = dictionary[cur];

                    var resCur = await cur;
                   
                    if (resCur)
                    {
                        var news = enumer.Current;
                        yield return news;
                    }
                    else
                    {
                        sourceEnumerators.Remove(enumer);
                    }

                    dictionary.Remove(cur);

                }*//*
            }

            foreach (var sour in s.Item1)
                Console.WriteLine($"{sour.IsCrawl}");*/
        }
    }
}
