using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
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
            throw new NotImplementedException();
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            throw new NotImplementedException();
        }

        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(int id)
        {
            return new ServiceActionResult<IEnumerable<Bike>>
            {
                Object = _context.Bikes
                    .Include(bike => bike.Station)
                    .Include(bike => bike.User)
                    .Where(bike => bike.Station.Id == id),
            };
        }

       
    }
}
