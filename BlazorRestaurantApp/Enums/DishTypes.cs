using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlazorRestaurantApp.Enums
{
    public enum DishTypes
    {
        [Display(Name = "Завтраки")]
        Breakfast,
        [Display(Name = "Супы")]
        Soup,
        [Display(Name = "Салаты")]
        Salad,
        [Display(Name = "Горячие блюда")]
        HotDish,
        [Display(Name = "Паста")]
        Pasta,
        [Display(Name = "Рыба")]
        Fish,
        [Display(Name = "Мясо")]
        Meat,
        [Display(Name = "Гарниры")]
        SideDishe,
        [Display(Name = "Десерты")]
        Desserts,
        [Display(Name = "Напитки")]
        Beverage,
        [Display(Name = "Отсутствует")]
        None
    }
}
