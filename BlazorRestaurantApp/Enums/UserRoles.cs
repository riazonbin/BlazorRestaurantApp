using System.ComponentModel.DataAnnotations;

namespace BlazorRestaurantApp.Enums
{
    public enum UserRoles
    {
        [Display(Name ="Клиент")]
        Customer,
        [Display(Name = "Сотрудник")]
        Employee
    }
}
