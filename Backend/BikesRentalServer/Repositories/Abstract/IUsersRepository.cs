using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IUsersRepository : IRepository<User>
    {
        User SetStatus(string id, UserStatus status);
    }
}
