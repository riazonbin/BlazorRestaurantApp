using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BlazorRestaurantApp.Data
{
    public class MenuItem
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ObjectId ImageId { get; set; }
    }
}
