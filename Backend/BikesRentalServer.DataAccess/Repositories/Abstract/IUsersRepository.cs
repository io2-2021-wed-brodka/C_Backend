using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IUsersRepository : IRepository<User>
    {
        IEnumerable<User> GetBlockedUsers();
        IEnumerable<User> GetAllTechs();
        User GetByUsername(string username);
        User GetByUsernameAndPassword(string username, string password);
        User SetStatus(int id, UserStatus status);
    }
}
