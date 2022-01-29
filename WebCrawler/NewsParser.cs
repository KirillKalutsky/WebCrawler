using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CoreModels;

namespace WebCrawler
{
    
    public class NewsParser
    {
        public static Event ParseHtmlPage(HtmlDocument page, Dictionary<string,string> pageElements) 
        {
            var result = new Event();
            foreach(var e in pageElements)
            {
                var p = result.GetType().GetProperty(e.Key);
                var value = new StringBuilder();
                
                var nodes = page.DocumentNode;
                var myNodes = nodes.SelectNodes(e.Value);

                if (myNodes != null)
                {
                    foreach (var node in myNodes)
                        value.Append(node.InnerText);
                }
                else
                    Debug.Print($"dont containt {e.Key} {e.Value}");
                
                p.SetValue(result, value.ToString().ReplaceHtmlTags(string.Empty));
            }
            return result;
        }
    }
}
