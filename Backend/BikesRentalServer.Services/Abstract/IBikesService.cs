using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        ServiceActionResult<IEnumerable<Bike>> GetAllBikes();
        ServiceActionResult<Bike> GetBike(string id);
        ServiceActionResult<Bike> AddBike(string stationId);
        ServiceActionResult<Bike> RemoveBike(string id);
        ServiceActionResult<Bike> RentBike(string id);
        ServiceActionResult<IEnumerable<Bike>> GetRentedBikes();
        ServiceActionResult<Bike> GiveBikeBack(string bikeId, string stationId);
        ServiceActionResult<Bike> BlockBike(string id);
        ServiceActionResult<Bike> UnblockBike(string id);
        ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes();
    }
}
