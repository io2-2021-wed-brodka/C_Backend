using System;
using System.Collections.Generic;

#nullable disable

namespace BikesRentalServer.Models
{
    public partial class Bike
    {
        public int BikeId { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public int? StationId { get; set; }
        public int? ReservationId { get; set; }
    }
}
