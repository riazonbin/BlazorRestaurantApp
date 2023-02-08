using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using BlazorRestaurantApp.Enums;

namespace BlazorRestaurantApp.Data
{
    public class RegistrationUserForm
    {

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль должен быть хотя бы 8 символов в длину", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string RepeatedPassword { get; set; }

        [Required]
        public UserRoles Role { get; set; }
    }
}
