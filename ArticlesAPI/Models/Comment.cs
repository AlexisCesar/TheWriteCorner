using MongoDB.Bson.Serialization.Attributes;

namespace ArticlesAPI.Models
{
    public class Comment
    {
        public string? Author { get; set; }
        [BsonElement("Text")]
        public string? Content { get; set; }
    }
}