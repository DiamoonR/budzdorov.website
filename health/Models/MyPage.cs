using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Pages")]
    public class MyPage
    {
        public int MyPageId { get; set; }

        [StringLength(150)]
        public string IdName { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Content { get; set; }

        public string Keywords { get; set; }
    }

    public class MyPageShort
    {
        public int MyPageId { get; set; }
        public string Name { get; set; }
        public string IdName { get; set; }
    }
}
