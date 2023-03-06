using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using BlazorRestaurantApp.Enums;

namespace BlazorRestaurantApp.Data
{
    public class RegistrationUserForm
    {

        [Required(ErrorMessage = "Фамилия должна быть заполнена!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Имя должно быть заполнено!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Отчество должно быть заполнено!")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Почта должна быть заполнена!")]
        [EmailAddress(ErrorMessage = "Не действительная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль должен быть заполнен!")]
        [StringLength(30, ErrorMessage = "Пароль должен быть хотя бы 8 символов в длину", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль!")]
        [Compare(nameof(Password), ErrorMessage ="Пароли не совпадают!")]
        public string RepeatedPassword { get; set; }

        [Required]
        public UserRoles Role { get; set; }
    }
}
