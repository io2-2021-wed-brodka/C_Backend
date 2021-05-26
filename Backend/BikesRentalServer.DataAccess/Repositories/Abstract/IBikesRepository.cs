using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IBikesRepository : IRepository<Bike>
    {
        IEnumerable<Bike> GetBlocked();
        Bike SetStatus(int id, BikeStatus status);
        Bike AssociateWithUser(int bikeId, int userId);
        Bike AssociateWithStation(int bikeId, int stationId);
    }
}
