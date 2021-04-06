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
            return _context.Stations.Include(Station => Station.Bikes).ToArray();
        }

        public Station GetStation(string id)
        {
            return _context.Stations.Include(Station => Station.Bikes).SingleOrDefault(s => s.Id.ToString() == id);
        }

        public IEnumerable<Bike> GetAllBikesAtStation(string id)
        {
            return _context.Bikes.Include(bike => bike.Station).Include(bike => bike.User).Where(bike => bike.Station.Id.ToString() == id);
        }

       
    }
}
