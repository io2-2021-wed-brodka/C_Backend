using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface IUsersService
    {
        User AddUser(string username, string password);
        User GetUserByUsernameAndPassword(string username, string password);
        string GenerateBearerToken(User user);
    }
}
