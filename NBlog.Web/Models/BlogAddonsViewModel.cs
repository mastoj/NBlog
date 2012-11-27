namespace NBlog.Web.Models
{
    public class BlogAddonsViewModel
    {
        public bool GoogleAnalyticsEnabled { get; set; }

        public string UAAccount { get; set; }

        public bool DisqusEnabled { get; set; }

        public string DisqusShortName { get; set; }
        public bool IsDebug { get; set; }
    }
}