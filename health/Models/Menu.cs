using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Menu")]
    public class Menu
    {
        public int MenuId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Url { get; set; }

        public int Rank { get; set; }
    }
}
