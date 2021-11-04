using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("PageInfo")]
    public class PageInfo
    {
        public PageInfo()
        {

        }
        public PageInfo(string _url)
        {
            PageInfoId = 0;
            Name = "";
            Description = "";
            Keywords = "";
            Content = "";
            Title = "";
            Url = _url;
        }
        public int PageInfoId { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(750)]
        public string Description { get; set; }
        [StringLength(450)]
        public string Keywords { get; set; }

        [DataType(DataType.Text)]
        public string Content { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(150)]
        public string Url { get; set; }
    }
}
