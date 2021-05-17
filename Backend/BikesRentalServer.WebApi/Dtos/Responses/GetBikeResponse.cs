using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetBikeResponse
    {
        public string Id { get; set; }
        public GetStationResponse Station { get; set; }
        public GetUserResponse User { get; set; }
        public BikeStatus Status { get; set; }
    }
}
