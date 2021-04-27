using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IUsersService
    {
        ServiceActionResult<User> AddUser(string username, string password);
        ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password);
        ServiceActionResult<string> GenerateBearerToken(User user);
        ServiceActionResult<IEnumerable<User>> GetAllUsers();
    }
}
