using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IStationsRepository : IRepository<Station>
    {
        IEnumerable<Station> GetActive();
        IEnumerable<Station> GetBlocked();
        Station SetStatus(string id, StationStatus status);
    }
}
