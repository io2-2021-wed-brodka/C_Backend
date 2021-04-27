using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Repositories
{
    public class BikesRepository : IBikesRepository
    {
        private readonly DatabaseContext _dbContext;
        
        public BikesRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<Bike> GetAll()
        {
            return _dbContext.Bikes.Include(b => b.Station).Include(b => b.User);
        }

        public Bike Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Bikes.Include(b => b.Station).Include(b => b.User).SingleOrDefault(b => b.Id == iid);
        }

        public Bike Add(Bike entity)
        {
            var bike = _dbContext.Bikes.Add(entity).Entity;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike Remove(string id)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            _dbContext.Bikes.Remove(bike);
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike Remove(Bike entity)
        {
            var bike = _dbContext.Bikes.Remove(entity).Entity;
            _dbContext.SaveChanges();

            return bike;
        }

        public IEnumerable<Bike> GetBlocked()
        {
            return _dbContext.Bikes.Where(b => b.Status == BikeStatus.Blocked).Include(b => b.Station).Include(b => b.User);
        }

        public Bike SetStatus(string id, BikeStatus status)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            bike.Status = status;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike Associate(string id, User user)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            bike.Station = null;
            bike.User = user;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike Associate(string id, Station station)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            bike.User = null;
            bike.Station = station;
            _dbContext.SaveChanges();

            return bike;
        }
    }
}
