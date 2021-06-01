﻿using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class Station
    {
        public const int BikeLimit = 10;
        
        public int Id { get; set; }
        public StationStatus Status { get; set; }
        public string Name { get; set; }
        public List<Bike> Bikes { get; set; }
    }

    public enum StationStatus
    {
        Active,
        Blocked,
    }
}
