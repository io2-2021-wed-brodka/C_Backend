using BikesRentalServer.Models;

namespace BikesRentalServer.Dtos.Requests
{
    public class AddBikeRequest
    {
        public string StationId { get; set; }
        public string BikeDescription { get; set; }
        public BikeStatus BikeStatus { get; set; }
    }
}
