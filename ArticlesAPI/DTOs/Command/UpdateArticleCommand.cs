using System.Text.Json.Serialization;

namespace ArticlesAPI.DTOs.Command
{
    public class UpdateArticleCommand
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IEnumerable<string> Authors { get; set; } = new List<string>();
        public DateTime? PublicationDate { get; set; }
        public IEnumerable<string> Keywords { get; set; } = new List<string>();
        [JsonIgnore]
        public DateTime LastUpdate => DateTime.Now;
        public int ReadingEstimatedTime { get; set; }
        public string? Abstract { get; set; }
    }
}
