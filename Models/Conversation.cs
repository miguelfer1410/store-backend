using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MyShopAPI.Models
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("product")]
        public Product Product { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } // ID do usuário que iniciou a conversa

        [BsonElement("messages")]
        public List<Message> Messages { get; set; } = new List<Message>(); // Lista de mensagens na conversa

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("deletedForUsers")]
        public List<string> DeletedForUsers { get; set; } = new List<string>();
    }
}
