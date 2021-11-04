using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Contraindications")]
    public class Contraindication
    {
        public int ContraindicationId { get; set; }
        public string Content { get; set; }
        public int HerbId { get; set; }
        public virtual Herb Herb { get; set; }
    }
}
