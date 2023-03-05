using BlazorRestaurantApp.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlazorRestaurantApp.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Фамилия должна быть заполнена!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Имя должно быть заполнено!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Отчество должно быть заполнено!")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Почта должна быть заполнена!")]
        [EmailAddress(ErrorMessage ="Не действительная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль должен быть заполнен!")]
        [StringLength(30, ErrorMessage = "Пароль должен быть хотя бы 8 символов в длину", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public UserRoles Role { get; set; }
    }
}
