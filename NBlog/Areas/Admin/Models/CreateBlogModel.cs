using System.ComponentModel.DataAnnotations;

namespace NBlog.Areas.Admin.Models
{
    public class CreateBlogModel
    {
        [Required]
        public string SubTitle { get; set; }
        [Required]
        public string BlogTitle { get; set; }
    }
}