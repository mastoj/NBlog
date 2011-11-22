using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NBlog.Domain;
using TJ.Extensions;

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public string TagsModel { get; set; }
        public string CategoriesModel { get; set; }
        public IList<string> Tags
        {
            get { return TagsModel.IsNotNull() ? TagsModel.Split(',').Select(y => y.Trim()).ToList() : new List<string>(); }
            set { TagsModel = value.IsNotNull() ? value.Aggregate((x, y) => x + ", " + y) : string.Empty; }
        }

        public IList<string> Categories
        {
            get { return CategoriesModel.IsNotNull() ? CategoriesModel.Split(',').Select(y => y.Trim()).ToList() : new List<string>(); }
            set { CategoriesModel = value.IsNotNull() ? value.Aggregate((x, y) => x + ", " + y) : string.Empty; }
        }
    }
}