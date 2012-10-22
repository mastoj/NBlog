using System.ComponentModel.DataAnnotations;

namespace NBlog.Web.Models
{
    public class CreateBlogModel
    {
        [Required]
        public string SubTitle { get; set; }
        [Required]
        public string BlogTitle { get; set; }
    }
}