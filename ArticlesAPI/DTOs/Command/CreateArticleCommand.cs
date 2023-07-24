using System.Text.Json.Serialization;
using ArticlesAPI.Models;

namespace ArticlesAPI.DTOs.Command
{
    public class CreateArticleCommand
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IEnumerable<string> Authors { get; set; } = new List<string>();
        public DateTime? PublicationDate { get; set; }
        public IEnumerable<string> Keywords { get; set; } = new List<string>();
        [JsonIgnore]
        public DateTime LastUpdate => DateTime.Now;
        public int ReadingEstimatedTime { get; set; }
        [JsonIgnore]
        public int LikeCount => 0;
        [JsonIgnore]
        public IEnumerable<Comment> Comments => new List<Comment>();
        public string? Abstract { get; set; }
    }
}
