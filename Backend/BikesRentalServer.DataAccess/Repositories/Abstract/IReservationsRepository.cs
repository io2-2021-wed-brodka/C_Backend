using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IReservationsRepository : IRepository<Reservation>
    {
        Reservation GetActiveReservation(int bikeId);
        IEnumerable<Reservation> GetActiveReservations(int userId);
    }
}
