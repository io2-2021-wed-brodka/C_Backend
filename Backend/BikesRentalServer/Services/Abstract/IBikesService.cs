using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesRentalServer.Services.Abstract
{
    public interface IBikesService
    {
        IEnumerable<Bike> GetAllBikes();
        Bike GetBike(string id);
    }
}
