using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace health.Models
{
    [Table("Disease")]
    public class Disease
    {
        public int DiseaseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string IdName { get; set; }
        public virtual IList<Recipe> Recipes { get; set; }
        public virtual Symptom Symptom { get; set; }

    }
    public class TDisease
    {
        public int DiseaseId { get; set; }
        public string Name { get; set; }
        public string IdName { get; set; }
        public string Description { get; set; }
        public int RecipeCount { get; set; }
    }
    public class DiseaseToList
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
