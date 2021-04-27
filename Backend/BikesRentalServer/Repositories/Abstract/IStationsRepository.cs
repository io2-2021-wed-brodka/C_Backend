using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IStationsRepository : IRepository<Station>
    {
        Station SetStatus(string id, StationStatus status);
    }
}
