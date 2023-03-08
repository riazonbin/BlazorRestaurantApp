using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlazorRestaurantApp.Enums
{
    public enum OrderStatuses
    {
        [Display(Name = "В процессе обработки")]
        InProgress,
        [Display(Name = "Заказ готовится на кухне")]
        IsCooking,
        [Display(Name = "Заказ приготовлен")]
        IsCooked,
        [Display(Name = "Заказ у официанта")]
        IsDelivering,
        [Display(Name = "Заказ доставлен")]
        IsDelivered
    }
}
