using System;

namespace NBlog.Web.Models
{
    public class AfterContentViewModel
    {
        public Guid PostId { get; set; }

        public bool DisqusEnabled { get; set; }

        public string Url { get; set; }

        public bool IsDebug { get; set; }
    }
}