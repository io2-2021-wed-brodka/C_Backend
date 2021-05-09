using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        #region Basics
        
        ServiceActionResult<IEnumerable<Bike>> GetAllBikes();
        ServiceActionResult<Bike> GetBike(string id);
        ServiceActionResult<Bike> AddBike(string stationId);
        ServiceActionResult<Bike> RemoveBike(string id);
        
        #endregion
        
        #region Renting
        
        ServiceActionResult<IEnumerable<Bike>> GetRentedBikes();
        ServiceActionResult<Bike> RentBike(string id);
        ServiceActionResult<Bike> GiveBikeBack(string bikeId, string stationId);
        
        #endregion
        
        #region Blocking
        
        ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes();
        ServiceActionResult<Bike> BlockBike(string id);
        ServiceActionResult<Bike> UnblockBike(string id);
        
        #endregion
        
        #region Reserving

        ServiceActionResult<IEnumerable<Bike>> GetReservedBikes();
        ServiceActionResult<Reservation> ReserveBike(string id);
        ServiceActionResult<Bike> CancelBikeReservation(string bikeId);

        #endregion
    }
}
