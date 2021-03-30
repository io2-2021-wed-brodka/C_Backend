using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Services
{
    public class BikesService : IBikesService
    {
        private readonly DatabaseContext _context;

        public BikesService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Bike> GetAllBikes()
        {
            return _context.Bikes.Include(bike => bike.Station).Include(bike => bike.User).ToArray();
        }

        public Bike GetBike(string id)
        {
            return _context.Bikes.Include(bike => bike.User).Include(bike => bike.Station).SingleOrDefault(b => b.Id.ToString() == id);
        }

        public Response<Bike> AddBike(AddBikeRequest request)
        {
            Response<Bike> response = new Response<Bike>();

            // Check if station exists and is active.
            var requestedStation = _context.Stations.Where(x => request.StationId == x.Id.ToString()).FirstOrDefault();
            if (requestedStation is null)
            {
                response.Message = "Station does not exist";
                return response;
            }
            if (requestedStation.Status == Models.BikeStationStatus.Blocked)
            {
                response.Message = "Requested station is blocked";
                return response;
            }

            Bike newBike = new Bike();
            newBike.Description = request.BikeDescription;
            newBike.Station = requestedStation;

            requestedStation.Bikes.Add(newBike);
            _context.Bikes.Add(newBike);
            _context.SaveChanges();

            response.Object = newBike;
            return response;
        }
    }
}
