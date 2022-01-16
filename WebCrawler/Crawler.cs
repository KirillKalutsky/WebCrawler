using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    public class Crawler
    {

        public async IAsyncEnumerable<Event> StartAsync(IEnumerable<Tuple<ICrawlableSource,string,int?>> sourcers)
        {
            var sourceEnumerators = sourcers
                .Select(e => e.Item1.CrawlAsync(e.Item2, e.Item3).GetAsyncEnumerator()).ToDictionary(x=>x);

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
                    
                }
                
            }
            
        }
    }
}
