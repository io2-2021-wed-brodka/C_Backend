using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Services
{
    public class BikesService : IBikesService
    {
        private readonly DatabaseContext _context;

        public BikesService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Bike> GetAllBikes()
        {
            return _context.Bikes.Include(bike => bike.Station).Include(bike => bike.User).ToArray();
        }

        public Bike GetBike(string id)
        {
            return _context.Bikes.Include(bike => bike.User).Include(bike => bike.Station).SingleOrDefault(b => b.Id.ToString() == id);
        }
    }
}
