using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyShopAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // Gera um novo ID se não for fornecido

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Images")]
        public List<string> ImageUrls { get; set; } = new List<string>();

        [BsonElement("Category")]
        public string Category { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Brand")]
        public string? Brand { get; set; }

        [BsonElement("ClothingType")]
        public string? ClothingType { get; set; }

        [BsonElement("Color")]
        public string? Color { get; set; }  // Novo campo
    }
}
