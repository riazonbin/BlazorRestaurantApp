using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlazorRestaurantApp.Data
{
    public class MenuItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ObjectId ImageId { get; set; }
    }
}
