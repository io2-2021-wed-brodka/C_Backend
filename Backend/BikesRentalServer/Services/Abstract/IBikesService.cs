using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        IEnumerable<Bike> GetAllBikes();
        Bike GetBike(string id);
        void AddBike(Bike bike);
    }
}
