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
        [StringLength(30, ErrorMessage = "Минимальное кол-во символов в фамилии 4, максимальное - 30!", MinimumLength = 4)]
        [RegularExpression("^[А-Яа-я]+$", ErrorMessage = "Фамилия не может содержать цифр или букв не на кириллице!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Имя должно быть заполнено!")]
        [StringLength(30, ErrorMessage = "Минимальное кол-во символов в имени 2, максимальное - 30!", MinimumLength = 2)]
        [RegularExpression("^[А-Яа-я]+$", ErrorMessage = "Имя не может содержать цифр или букв не на кириллице!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Отчество должно быть заполнено!")]
        [StringLength(30, ErrorMessage = "Минимальное кол-во символов в отчестве 4, максимальное - 30!", MinimumLength = 4)]
        [RegularExpression("^[А-Яа-я]+$", ErrorMessage = "Отчество не может содержать цифр или букв не на кириллице!")]
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
