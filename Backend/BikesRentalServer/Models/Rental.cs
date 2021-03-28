using System;

namespace BikesRentalServer.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public Bike Bike { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
