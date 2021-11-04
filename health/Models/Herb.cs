using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Herbs")]
    public class Herb
    {
        public int HerbId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string IdName { get; set; }
        public virtual IList<Contraindication> Contraindications { get; set; }
    }

    public class THerb
    {
        public int HerbId { get; set; }
        public string Name { get; set; }
        public string IdName { get; set; }
        public string Description { get; set; }
    }

    public class HerbToList
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
