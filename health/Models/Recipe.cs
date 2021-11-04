using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace health.Models
{
    [Table("Recipes")]
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Content { get; set; }
        public int DiseaseId { get; set; }
        public int Rank { get; set; }

        public virtual Disease Disease { get; set; }
    }
    public class TRecipe
    {
        public string Description { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
