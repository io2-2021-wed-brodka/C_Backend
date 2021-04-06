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

        public IEnumerable<Station> GetAllStations()
        {
            return _context.Stations.ToArray();
        }

        public Station GetStation(string id)
        {
            return _context.Stations.SingleOrDefault(s => s.Id.ToString() == id);
        }

        public IEnumerable<Bike> GetAllBikesAtStation(string id)
        {
            return _context.Stations.Include(s => s.Bikes).FirstOrDefault(s => s.Id.ToString() == id)?.Bikes;
        }

       
    }
}
