using System;
using System.Collections.Generic;

namespace NewsReader.Model;

public class DisplayArticle
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; }
    public DateTime PublishedOn { get; set; }
    public string Content { get; set; }
    public string Link { get; set; }
}