using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IStationsRepository : IRepository<Station>
    {
        IEnumerable<Station> GetActive();
        IEnumerable<Station> GetBlocked();
        Station SetStatus(string id, StationStatus status);
        Station SetStatus(int id, StationStatus status);
    }
}
