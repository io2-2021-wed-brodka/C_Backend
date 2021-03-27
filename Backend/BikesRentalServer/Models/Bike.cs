namespace BikesRentalServer.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public BikeStatus Status { get; set; }
        public string Description { get; set; }
        public User User { get; set; } // Can be null if station is assigned
        public Station Station { get; set; } // Can be null if user is assigned
    }

    public enum BikeStatus
    {
        Working,
        InService,
        Blocked
    }
}
