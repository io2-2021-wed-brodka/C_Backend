using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        IEnumerable<Bike> GetAllBikes();
        Bike GetBike(string id);
        Response<Bike> AddBike(AddBikeRequest request);
    }
}
