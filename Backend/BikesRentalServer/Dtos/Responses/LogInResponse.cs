using BikesRentalServer.Models;

namespace BikesRentalServer.Dtos.Responses
{
    public class LogInResponse
    {
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
