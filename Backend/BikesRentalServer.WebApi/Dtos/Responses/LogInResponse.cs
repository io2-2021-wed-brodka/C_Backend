using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class LogInResponse
    {
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
