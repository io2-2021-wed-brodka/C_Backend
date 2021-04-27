﻿using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Reservation> Reservations { get; set; }
    }

    public enum UserStatus
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
