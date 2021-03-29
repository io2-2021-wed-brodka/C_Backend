using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserState State { get; set; }
        public UserRole Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Rental> Rentals { get; set; }
    }

    public enum UserState
    {
        Active,
        Banned,
    }

    public enum UserRole
    {
        User,
        Tech,
        Admin,
    }
}
