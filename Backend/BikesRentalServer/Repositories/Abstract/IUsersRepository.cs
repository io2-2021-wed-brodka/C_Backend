using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User GetByUsernameAndPassword(string username, string password);
        User SetStatus(string id, UserStatus status);
        IEnumerable<Bike> GetRentedBikes(string id);
    }
}
