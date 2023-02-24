using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using BlazorRestaurantApp.Enums;

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
        [RegularExpression("^[0-9]*$")]
        public decimal Price { get; set; }

        public ObjectId ImageId { get; set; }

        [Required]
        public DishTypes DishType { get; set; }
    }
}
