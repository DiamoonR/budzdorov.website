using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Articles")]
    public class Article
    {
        public int ArticleId { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(350)]
        public string Description { get; set; }
        public string Content { get; set; }
        public string IdName { get; set; }
        public int Rank { get; set; }
    }

    public class TArticle
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public string IdName { get; set; }
    }
}
