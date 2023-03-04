using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorRestaurantApp.Data
{
    public class Table
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int TableNumber { get; set; }

        public Reservation TableReservation { get; set; }
    }
}
