using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace health.Models
{
    [Table("Symptoms")]
    public class Symptom
    {
        public int SymptomId { get; set; }

        public string Content { get; set; }
        public int DiseaseId { get; set; }

        public virtual Disease Disease { get; set; }
    }
}
