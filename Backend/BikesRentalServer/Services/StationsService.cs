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
        private readonly DatabaseContext _context;

        public StationsService(DatabaseContext context)
        {
            _context = context;
        }

        public ServiceActionResult<IEnumerable<Station>> GetAllStations()
        {
            var result = _context.Stations.AsEnumerable();
            return ServiceActionResult.Success(result);
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            var station = _context.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
            {
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            }

            return ServiceActionResult.Success(station);
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id)
        {
            var station = _context.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
            {
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");
            }

            return ServiceActionResult.Success(station.Bikes.AsEnumerable());
        }
    }
}
