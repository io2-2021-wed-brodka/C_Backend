using System;
using System.Collections.Generic;
using System.Linq;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;

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
            throw new NotImplementedException();
        }

        public Station GetStation(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bike> GetAllBikesAtStation(int id)
        {
            var bikes = from b in _context.Bikes.Include(s => s.Station) where b.Station.Id == id select b;
            
            return bikes;
        }

       
    }
}
