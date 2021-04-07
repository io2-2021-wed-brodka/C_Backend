using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        ServiceActionResult<IEnumerable<Bike>> GetAllBikes();
        ServiceActionResult<Bike> GetBike(string id);
        ServiceActionResult<Bike> AddBike(AddBikeRequest request);
        ServiceActionResult<Bike> RemoveBike(string id);
        ServiceActionResult<Bike> RentBike(RentBikeRequest request);
    }
}
