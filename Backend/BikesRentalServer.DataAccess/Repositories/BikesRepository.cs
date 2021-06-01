using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.DataAccess.Repositories
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
            return _dbContext.Bikes
                .Include(b => b.Station)
                .ThenInclude(s => s.Bikes)
                .Include(b => b.User)
                .Include(b => b.Malfunctions);
        }

        public Bike Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Bikes
                .Include(b => b.Station)
                .ThenInclude(s => s.Bikes)
                .Include(b => b.User)
                .Include(b => b.Malfunctions)
                .SingleOrDefault(b => b.Id == iid);
        }

        public Bike Add(Bike entity)
        {
            var bike = _dbContext.Bikes.Add(entity).Entity;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike Remove(int id)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            _dbContext.Bikes.Remove(bike);
            _dbContext.SaveChanges();

            return bike;
        }

        public IEnumerable<Bike> GetBlocked()
        {
            return _dbContext.Bikes
                .Where(b => b.Status == BikeStatus.Blocked)
                .Include(b => b.Station)
                .Include(b => b.User)
                .Include(b => b.Malfunctions);
        }

        public Bike SetStatus(int id, BikeStatus status)
        {
            var bike = Get(id);
            if (bike is null)
                return null;

            bike.Status = status;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike AssociateWithUser(int bikeId, int userId)
        {
            var bike = Get(bikeId);
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == userId);
            if (bike is null || user is null)
                return null;

            bike.Station = null;
            bike.User = user;
            _dbContext.SaveChanges();

            return bike;
        }

        public Bike AssociateWithStation(int bikeId, int stationId)
        {
            var bike = Get(bikeId);
            var station = _dbContext.Stations.SingleOrDefault(u => u.Id == stationId);
            if (bike is null || station is null)
                return null;

            bike.User = null;
            bike.Station = station;
            _dbContext.SaveChanges();

            return bike;
        }

        private Bike Get(int id) => Get(id.ToString());
    }
}
