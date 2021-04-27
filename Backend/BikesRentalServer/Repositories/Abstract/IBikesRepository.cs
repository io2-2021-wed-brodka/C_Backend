using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IBikesRepository : IRepository<Bike>
    {
        Bike SetStatus(string id, BikeStatus status);
    }
}
