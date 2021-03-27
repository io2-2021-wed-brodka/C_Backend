﻿namespace BikesRentalServer.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public BikeState State { get; set; }
        public string Description { get; set; }
        public User User { get; set; } // Can be null if station is assigned
        public BikeStation Station { get; set; } // Can be null if user is assigned
    }

    public enum BikeState
    {
        Working,
        InService,
        Blocked
    }
}
