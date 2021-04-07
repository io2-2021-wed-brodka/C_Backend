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
            var station = _context.Stations
                .Include(s => s.Bikes)
                .FirstOrDefault(x => request.StationId == x.Id.ToString());
            if (station is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Station does not exist",
                    Status = Status.EntityNotFound,
                };
            }
            if (station.Status is BikeStationStatus.Blocked)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Requested station is blocked",
                    Status = Status.InvalidStatus,
                };
            }

            var newBike = new Bike
            {
                Description = string.Empty,
                Station = station,
            };

            station.Bikes.Add(newBike);
            _context.Bikes.Add(newBike);
            _context.SaveChanges();

            return new ServiceActionResult<Bike>
            {
                Object = newBike,
                Status = Status.Success,
            };
        }

        public ServiceActionResult<Bike> RemoveBike(RemoveBikeRequest request)
        {
            var bike = _context.Bikes.FirstOrDefault(b => b.Id.ToString() == request.BikeId);
            if (bike is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike not found",
                    Status = Status.EntityNotFound,
                };
            }
            if (bike.Status != BikeStatus.Blocked)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike not blocked",
                    Status = Status.InvalidStatus,
                };
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

            return new ServiceActionResult<Bike>
            {
                Object = bike,
                Status = Status.Success,
            };
        }
    }
}
