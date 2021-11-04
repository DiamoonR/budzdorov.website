using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace health.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int CommentId { get; set; }

        public string UserName { get; set; }

        public DateTime DateAdd { get; set; }
        public string Message { get; set; }
        public string Razdel { get; set; }
        public int ElementId { get; set; }
        public bool Enable { get; set; }
    }
}
