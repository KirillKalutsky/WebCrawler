using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreModels;
using log4net;

namespace WebCrawler
{
    public class Crawler
    {

        private readonly ILog logger;
        private readonly IHttpClientFactory clientFactory;
        public Crawler(ILog logger, IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this.clientFactory = clientFactory;
        }
        public Crawler()
        {
        }

        public async IAsyncEnumerable<Event> StartAsync(IEnumerable<Tuple<CrawlableSource,string,int?>> sourcers)
        {
            logger.Info("Начало обхода");
            var sourceEnumerators = sourcers
                .Select(e => e.Item1.CrawlAsync(e.Item2, e.Item3, clientFactory.CreateClient()).GetAsyncEnumerator())
                .ToList();

            var dict = new Dictionary<Task<bool>, IAsyncEnumerator<Event>>();

            while (sourceEnumerators.Any())
            {
                for (var i = 0; i < sourceEnumerators.Count; i++)
                {
                    var enumerator = sourceEnumerators[i];
                    var task = enumerator.MoveNextAsync().AsTask();

                    dict[task] = enumerator;
                }

                while (dict.Any())
                {
                    var fastTask = await Task.WhenAny(dict.Keys);

                    if (fastTask.IsCompletedSuccessfully)
                    {
                        var flag = await fastTask;
                        if (flag)
                        {
                            var news = dict[fastTask].Current;
                            yield return news;
                        }
                        else
                        {
                            sourceEnumerators.Remove(dict[fastTask]);
                        }
                    }

                    dict.Remove(fastTask);
                }
            }
        }
    }
}
