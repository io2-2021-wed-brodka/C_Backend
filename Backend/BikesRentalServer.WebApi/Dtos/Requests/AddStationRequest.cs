namespace BikesRentalServer.WebApi.Dtos.Requests
{
    public class AddStationRequest
    {
        public string Name { get; set; }
        public int? BikesLimit { get; set; }
    }
}
