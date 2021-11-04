using health.HelperClasses;
using health.Models;
using System.Collections.Generic;

namespace health.ViewModels
{
    public class DiseaseViewModel
    {
        public List<TDisease> Diseases { get; set; }
        public PageInfo PageInfo { get; set; }
        public Crumb Crumb { get; set; }
        public Pagination Pager { get; set; }        
    }
}
