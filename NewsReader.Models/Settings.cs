using System.Collections.Generic;

namespace NewsReader.Models
{
    public class Settings
    {
        public static string AppSettings = "9EFAFBF9-0005-4F14-A8D1-762A232D7B95";
        public MainWindowInfo MainWindowInfo { get; set; } = new MainWindowInfo();

        public string VoiceType { get; set; }
        public string Theme { get; set; }
        public int RefreshInterval { get; set; }
        public List<string> Feeds { get; set; } = new List<string>();
        public List<Article> BookMarkedArticles { get; set; } = new List<Article>();
    }
}