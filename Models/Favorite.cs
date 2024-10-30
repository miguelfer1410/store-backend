using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Favorite
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("UserId")]
    public string UserId { get; set; }

    [BsonElement("ProductId")]
    public string ProductId { get; set; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 