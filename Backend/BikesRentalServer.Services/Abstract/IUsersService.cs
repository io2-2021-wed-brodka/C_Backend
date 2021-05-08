using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IUsersService
    {
        #region Basics
        
        ServiceActionResult<IEnumerable<User>> GetAllUsers();
        ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password);
        ServiceActionResult<User> AddUser(string username, string password);
        
        ServiceActionResult<string> GenerateBearerToken(User user);
        
        #endregion
        
        #region Blocking
        
        ServiceActionResult<User> BlockUser(string userId);
        ServiceActionResult<User> UnblockUser(string userId);
        
        #endregion
    }
}
