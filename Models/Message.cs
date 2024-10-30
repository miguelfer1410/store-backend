using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MyShopAPI.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("isFromAdmin")]
        public bool IsFromAdmin { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
}
