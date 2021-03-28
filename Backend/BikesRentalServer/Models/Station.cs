using System.Collections.Generic;

namespace BikesRentalServer.Models
{
    public class Station
    {
        public int Id { get; set; }
        public BikeStationStatus Status { get; set; }
        public string Name { get; set; }
        public List<Bike> Bikes { get; set; }
    }

    public enum BikeStationStatus
    {
        Working,
        Blocked,
    }
}
