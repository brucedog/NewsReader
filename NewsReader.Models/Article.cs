using System;
using System.Collections.Generic;

namespace NewsReader.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public List<string> Categories { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsRead { get; set; }
        public bool IsBookMarkedForLater { get; set; }
    }
}
