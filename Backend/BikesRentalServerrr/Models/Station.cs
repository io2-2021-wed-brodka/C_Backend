using System;
using System.Collections.Generic;

#nullable disable

namespace BikesRentalServer.Models
{
    public partial class Station
    {
        public int StationId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int StationedBikesId { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
    }
}
