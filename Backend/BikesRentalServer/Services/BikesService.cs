using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Controllers;
namespace BikesRentalServer.Services
{
    public class BikesService : IBikesService
    {
        
        public IEnumerable<Bike> GetAllBikes()
        {
            return new List<Bike>{
                new Bike(){Id=1, Description="Description", State = BikeState.Working, Station = null, User = null }
            };
        }

        public Bike GetBike(string id)
        {
            throw new NotImplementedException();
        }
    }
}
