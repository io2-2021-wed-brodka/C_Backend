using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IBikesRepository : IRepository<Bike>
    {
        IEnumerable<Bike> GetBlocked();
        Bike SetStatus(int id, BikeStatus status);
        Bike Associate(int id, User user);
        Bike Associate(int id, Station station);
    }
}
