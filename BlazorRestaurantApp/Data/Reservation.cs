using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorRestaurantApp.Data
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ReservedCustomerId { get; set; }

        public DateTime TimeOfReservation { get; set; }

        public DateTime EndTimeOfReservation { get; set; }

        public Reservation()
        {
            EndTimeOfReservation = TimeOfReservation.AddHours(3);
        }
    }
}
