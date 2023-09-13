using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public IEnumerable<string> Authors { get; set; } = new List<string>();
    public DateTime? PublicationDate { get; set; }
    public IEnumerable<string> Keywords { get; set; } = new List<string>();
    public DateTime LastUpdate { get; set; }
    public int ReadingEstimatedTime { get; set; }
    public int LikeCount { get; set; }
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    public string? Abstract { get; set; }
}