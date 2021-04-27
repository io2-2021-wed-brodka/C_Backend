using BikesRentalServer.Models;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IReservationsRepository : IRepository<Reservation>
    {
        Reservation GetActiveReservation(string bikeId);
    }
}
