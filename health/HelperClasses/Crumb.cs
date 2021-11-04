using System.Collections.Generic;

namespace health.HelperClasses
{
    public class Crumb
    {
        public Crumb()
        {
            List = new Dictionary<string, string>();
        }
        public Dictionary<string, string> List { get; set; }
    }
}
