using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class BikeStation
    {
        public int Id { get; set; }
        public BikeStationState State { get; set; }
        public string Location { get; set; }
        public List<Bike> Bikes { get; set; }
    }

    public enum BikeStationState
    {
        Working,
        Blocked
    }
}
