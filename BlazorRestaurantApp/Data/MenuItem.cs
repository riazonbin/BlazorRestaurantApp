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

        [Required(ErrorMessage = "Название блюда должно быть заполнено!")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Цена блюда должно быть заполнена!")]
        public decimal Price { get; set; }

        public ObjectId ImageId { get; set; }

        [Required(ErrorMessage = "Тип блюда должен быть указан!")]
        public DishTypes DishType { get; set; }

        [Required(ErrorMessage = "Вес блюда должно быть указан!")]
        public int Weight { get; set; }
    }
}
