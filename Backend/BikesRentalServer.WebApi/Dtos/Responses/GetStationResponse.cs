using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetStationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StationStatus Status { get; set; }
        public int ActiveBikesCount { get; set; }
        public int ReservationsCount { get; set; }
        public int MalfunctionsCount { get; set; }
        public int BikesLimit { get; set; }
    }

}
