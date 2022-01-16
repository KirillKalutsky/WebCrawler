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
    public class PageLoader
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<Tuple<HttpResponseMessage,string>> LoadPageAsync(string url, int numberOfAttempts = 5)
        {
            HttpResponseMessage result;
            try
            {
                result = await httpClient.GetAsync(url);
            }
            catch
            {
                Console.WriteLine("Ошибка при загрузке");
                Debug.Print("Ошибка при загрузке");
                result = new HttpResponseMessage();
                result.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            if (!result.IsSuccessStatusCode && numberOfAttempts > 0)
            {
                Console.WriteLine($"BadStatusCode {numberOfAttempts}");
                Console.WriteLine($"{url}");
                await Task.Delay(500);
                result = (await LoadPageAsync(url, numberOfAttempts - 1)).Item1;
            }
            Console.WriteLine($"AllGood {numberOfAttempts}");
            return Tuple.Create(result,url);
        }



        public static async Task<IEnumerable<string>> GetPageElementAsync(string url, HtmlElement link)
        {
            var result = new List<string>();

            var body = (await PageLoader.LoadPageAsync(url)).Item1;

            if (body.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await body.Content.ReadAsStringAsync());

                var searchElements = doc.DocumentNode.SelectNodes(link.XPath);

                if (searchElements != null)
                    foreach (var e in searchElements)
                    {
                        var l = e.GetAttributeValue(link.AttributeName, "");
                        result.Add(l);
                    }
                else
                {
                    Console.WriteLine($"{url} : элементы не найдены");
                    Debug.Print($"{url} : элементы не найдены");
                }
            }
            return result;
        }
    }
}
