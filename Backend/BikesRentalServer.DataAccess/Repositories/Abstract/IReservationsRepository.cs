using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IReservationsRepository : IRepository<Reservation>
    {
        Reservation GetActiveReservation(string bikeId);
        Reservation GetActiveReservation(int bikeId);
        IEnumerable<Reservation> GetActiveReservations(string userId);
        IEnumerable<Reservation> GetActiveReservations(int userId);
    }
}
