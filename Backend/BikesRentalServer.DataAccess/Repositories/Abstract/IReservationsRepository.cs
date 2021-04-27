using BikesRentalServer.Models;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IReservationsRepository : IRepository<Reservation>
    {
        Reservation GetActiveReservation(string bikeId);
    }
}
