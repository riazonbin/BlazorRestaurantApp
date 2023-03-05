using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlazorRestaurantApp.Data
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public string CustomerId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OrderStartTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OrderEndTime { get; set; }

        public string TableId { get; set; }
    }
}
