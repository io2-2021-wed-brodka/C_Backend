using System;
using System.Collections.Generic;

#nullable disable

namespace BikesRentalServer.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public int RentedBikesId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
    }
}
