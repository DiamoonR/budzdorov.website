namespace health.HelperClasses
{
    public class Pagination
    {
        public string Url { get; set; }
        public int CurrentPage { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }
        public int MaxSize { get; set; }
        public int NumberSeria { get; set; }
        public int PageSize { get; set; }
        public string Query { get; set; }
        public string AdditionalString { get; set; }
    }
}
