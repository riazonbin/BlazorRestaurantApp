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

        public string TableId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartTimeOfReservation { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndTimeOfReservation { get; set; }

        public Reservation(DateTime startTimeOfReservation)
        {
            StartTimeOfReservation = startTimeOfReservation;
            EndTimeOfReservation = StartTimeOfReservation.AddMinutes(4);
        }
    }
}
