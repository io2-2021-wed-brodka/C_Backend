using System;

namespace BikesRentalServer.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public User User { get; set; }
        public Bike Bike { get; set; }
    }
}
