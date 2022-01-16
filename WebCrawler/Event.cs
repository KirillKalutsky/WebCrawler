using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Event
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Date{ get; set; }
        public string Body { get; set; }
        public override int GetHashCode()
        {
            return Link.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Event))
                return false;
            
            return ((Event)obj).Link.Equals(Link);
        }

        public override string ToString()
        {
            return $"{Link}\n{Body}";
        }
    }
}
