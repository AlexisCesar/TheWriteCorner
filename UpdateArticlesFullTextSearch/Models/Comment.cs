﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UpdateArticlesFullTextSearch.Models
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