using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
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

        public ServiceActionResult<IEnumerable<Bike>> GetAllBikes()
        {
            return new ServiceActionResult<IEnumerable<Bike>>
            {
                Object = _context.Bikes.Include(bike => bike.Station).Include(bike => bike.User),
            };
        }

        public ServiceActionResult<Bike> GetBike(string id)
        {
            return new ServiceActionResult<Bike>
            {
                Object = _context.Bikes
                    .Include(bike => bike.User)
                    .Include(bike => bike.Station)
                    .SingleOrDefault(b => b.Id.ToString() == id),
            };
        }

        public ServiceActionResult<Bike> AddBike(AddBikeRequest request)
        {
            var response = new ServiceActionResult<Bike>();

            var station = _context.Stations.FirstOrDefault(x => request.StationId == x.Id.ToString());
            if (station is null)
            {
                response.Message = "Station does not exist";
                return response;
            }
            if (station.Status is BikeStationStatus.Blocked)
            {
                response.Message = "Requested station is blocked";
                return response;
            }

            var newBike = new Bike
            {
                Description = string.Empty,
                Station = station,
            };

            station.Bikes.Add(newBike);
            _context.Bikes.Add(newBike);
            _context.SaveChanges();

            response.Object = newBike;
            return response;
        }

        public ServiceActionResult<Bike> RemoveBike(RemoveBikeRequest request)
        {
            var response = new ServiceActionResult<Bike>();

            var bike = _context.Bikes.FirstOrDefault(b => b.Id.ToString() == request.BikeId);
            if (bike is null)
            {
                response.Message = "Bike not found";
                return response;
            }
            if (bike.Status != BikeStatus.Blocked)
            {
                response.Message = "Bike not blocked";
                return response;
            }

            if (bike.User is not null)
            {
                var rental = _context.Rentals.FirstOrDefault(r => r.Bike == bike);
                if (rental is null)
                    throw new InvalidOperationException("Missing rental entry");
                
                _context.Rentals.Remove(rental);
            }
            
            foreach (var reservation in _context.Reservations.Where(r => r.Bike == bike))
                _context.Reservations.Remove(reservation);

            _context.Bikes.Remove(bike);
            _context.SaveChanges();
            
            response.Object = bike;
            return response;
        }
    }
}
