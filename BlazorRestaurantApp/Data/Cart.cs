using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorRestaurantApp.Data
{
    public class Cart
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
