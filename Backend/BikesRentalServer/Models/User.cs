using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public UserState State { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Rental> Rentals { get; set; }
    }

    public enum UserState
    {
        Active,
        Banned
    }
}
