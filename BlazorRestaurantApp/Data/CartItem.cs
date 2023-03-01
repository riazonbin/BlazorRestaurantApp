using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorRestaurantApp.Data
{
    public class CartItem
    {
        public MenuItem? MenuItem { get; set; }
        public int Quantity { get; set; }
    }
}
