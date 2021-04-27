using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User SetStatus(string id, UserStatus status);
    }
}
