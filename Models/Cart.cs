using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyShopAPI.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Product.Price);
        }
    }
}
