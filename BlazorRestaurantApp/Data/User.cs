﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlazorRestaurantApp.Data
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

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
        public string Role { get; set; }
    }
}