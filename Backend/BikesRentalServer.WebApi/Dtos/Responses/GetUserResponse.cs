using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetUserResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserStatus Status { get; set; }
    }
}
