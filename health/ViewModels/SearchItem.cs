using System.Text.Json.Serialization;

namespace health.ViewModels
{
    public class SearchItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
