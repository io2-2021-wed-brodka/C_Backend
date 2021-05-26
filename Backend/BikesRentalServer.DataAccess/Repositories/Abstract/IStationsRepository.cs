using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IStationsRepository : IRepository<Station>
    {
        IEnumerable<Station> GetActive();
        IEnumerable<Station> GetBlocked();
        Station SetStatus(int id, StationStatus status);
    }
}
