using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArticlesAPI.DTOs.Command
{
    public class CreateCommentCommand
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        public string? Author { get; set; }
        [BsonElement("Text")]
        public string? Content { get; set; }
    }
}

