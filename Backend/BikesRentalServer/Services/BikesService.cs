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
            return _context.Bikes.Include(s => s.Station).Include(u => u.User).ToArray();
        }

        public Bike GetBike(string id)
        {
            var bikes = from b in _context.Bikes.Include(s=> s.Station).Include(u=> u.User) where b.Id.ToString() == id select b;
            return bikes.SingleOrDefault();
        }
    }
}
