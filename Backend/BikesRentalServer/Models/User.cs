using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class User
    {
        public const int RentalLimit = 4;
        public const int ReservationLimit = 3;
        
        public int Id { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Bike> RentedBikes { get; set; }
    }  

    public enum UserStatus
    {
        Active,
        Blocked,
    }

    public enum UserRole
    {
        User,
        Tech,
        Admin,
    }
}
