﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NBlog.Data;

namespace NBlog.Areas.Admin.Models
{
    public class PostViewModel : Entity, IPost
    {
        public string ShortUrl { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Publish { get; set; }
        public DateTime? PublishDate { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> Categories { get; set; }
    }
}