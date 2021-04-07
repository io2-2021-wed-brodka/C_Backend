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
            return new ServiceActionResult<IEnumerable<Station>>
            {
                Object = _context.Stations.ToArray(),
                Status = Status.Success,
            };
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            var station = _context.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
            {
                return new ServiceActionResult<Station>
                {
                    Message = "Station not found.",
                    Status = Status.EntityNotFound,
                };
            }
            
            return new ServiceActionResult<Station>
            {
                Object = station,
                Status = Status.Success,
            };
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id)
        {
            var station = _context.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
            if (station is null)
            {
                return new ServiceActionResult<IEnumerable<Bike>>
                {
                    Message = "Station not found.",
                    Status = Status.EntityNotFound,
                };
            }
            
            return new ServiceActionResult<IEnumerable<Bike>>
            {
                Object = station.Bikes,
                Status = Status.Success,
            };
        }

       
    }
}
