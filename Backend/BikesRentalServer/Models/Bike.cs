namespace BikesRentalServer.Models
{
    /// <summary>
    /// Represents bike.
    /// </summary>
    public class Bike
    {
        public const BikeStatus DefaultBikeStatus = BikeStatus.Working;
        
        /// <summary>
        /// Bike's database ID.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Enum indicating current bike's status.
        /// </summary>
        public BikeStatus Status { get; set; }
        
        /// <summary>
        /// Optional description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// User currently renting this bike. Can be null if bike is not rented.
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Station at which bike currently is. Can be null if bike is rented.
        /// </summary>
        public Station Station { get; set; }
    }

    public enum BikeStatus
    {
        Working,
        Blocked,
    }
}
