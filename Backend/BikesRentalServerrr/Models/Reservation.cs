using System;
using System.Collections.Generic;

#nullable disable

namespace BikesRentalServer.Models
{
    public partial class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationEndTime { get; set; }
    }
}
