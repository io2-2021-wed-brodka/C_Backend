using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Repositories
{
    public class StationsRepository : IStationsRepository
    {
        private readonly DatabaseContext _dbContext;
        
        public StationsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Station> GetAll()
        {
            return _dbContext.Stations;
        }

        public Station Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Stations.SingleOrDefault(s => s.Id == iid);
        }

        public Station Add(Station entity)
        {
            var station = _dbContext.Stations.Add(entity).Entity;
            _dbContext.SaveChanges();

            return station;
        }

        public Station Remove(string id)
        {
            var station = Get(id);
            if (station is null)
                return null;

            _dbContext.Stations.Remove(station);
            _dbContext.SaveChanges();

            return station;
        }

        public Station Remove(Station entity)
        {
            var station = _dbContext.Stations.Remove(entity).Entity;
            _dbContext.SaveChanges();

            return station;
        }

        public IEnumerable<Station> GetActive()
        {
            return _dbContext.Stations.Where(s => s.Status == StationStatus.Working);
        }
        
        public IEnumerable<Station> GetBlocked()
        {
            return _dbContext.Stations.Where(s => s.Status == StationStatus.Blocked);
        }

        public Station SetStatus(string id, StationStatus status)
        {
            var station = Get(id);
            if (station is null)
                return null;

            station.Status = status;
            _dbContext.SaveChanges();

            return station;
        }
    }
}
