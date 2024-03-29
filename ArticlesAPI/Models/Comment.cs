﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArticlesAPI.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Author { get; set; }
        [BsonElement("Text")]
        public string? Content { get; set; }
    }
}