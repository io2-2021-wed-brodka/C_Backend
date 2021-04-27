using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IBikesRepository : IRepository<Bike>
    {
        Bike SetStatus(string id, BikeStatus status);
        Bike Associate(string id, User user);
        Bike Associate(string id, Station station);
    }
}
