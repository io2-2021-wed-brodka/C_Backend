using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Services
{
    public class StationsService : IStationsService
    {
        private readonly DatabaseContext _dbContext;

        public StationsService(DatabaseContext context)
        {
            _dbContext = context;
        }

        public ServiceActionResult<IEnumerable<Station>> GetAllStations()
        {
            var result = _dbContext.Stations.AsEnumerable();
            return ServiceActionResult.Success(result);
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            return ServiceActionResult.Success(station);
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id)
        {
            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");
            return ServiceActionResult.Success(station.Bikes.AsEnumerable());
        }

        public ServiceActionResult<Station> RemoveStation(string id)
        {
            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Bikes.Count > 0)
                return ServiceActionResult.InvalidState<Station>("Station has bikes");

            _dbContext.Stations.Remove(station);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(station);
        }

        
        public ServiceActionResult<Station> AddStation(AddStationRequest request)
        {
            var newStation = new Station
            {
                Name = request.Name,
                Status=BikeStationStatus.Working,
            };

            _dbContext.Stations.Add(newStation);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(newStation);
        }
    }
}
