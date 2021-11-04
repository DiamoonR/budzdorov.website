using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Images")]
    public class UploadImage
    {
        public int UploadImageId { get; set; }

        [StringLength(50)]
        public string Alt { get; set; }

        [StringLength(150)]
        public string Url { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }
        public bool EnableBigPhoto { get; set; }
    }
}
