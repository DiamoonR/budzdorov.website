using health.Models;
using System.Collections.Generic;

namespace health.ViewModels
{
    public class HomeViewModel
    {
        public PageInfo PageInfo { get; set; }        
        public List<TArticle> LastArticles { get; set; }
        public List<TRecipe> LastRecipes { get; set; }
    }
}
