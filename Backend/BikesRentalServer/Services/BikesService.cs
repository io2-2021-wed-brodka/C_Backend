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

        public Response<Bike> RemoveBike(RemoveBikeRequest request)
        {
            Response<Bike> response = new Response<Bike>();

            var requestedBike = _context.Bikes.Where(b => b.Id.ToString() == request.BikeId).FirstOrDefault();

            // Check if bike exists.
            if (requestedBike is null)
            {
                response.Message = "Bike not found";
                return response;
            }
            // Check if bike is blocked
            if(requestedBike.Status != BikeStatus.Blocked)
            {
                response.Message = "Bike not blocked";
                return response;
            }

            // If bike is rented, remove it from rentals.
            if(requestedBike.User != null)
            {
                var rental = _context.Rentals.Where(r => r.Bike == requestedBike).FirstOrDefault();
                if(rental is null)
                {
                    Console.WriteLine("Should never happen.");
                }
                else
                {
                    _context.Rentals.Remove(rental); // This should auto remove it from Users rental list.
                }
            }
            // Remove bike from all reservations.
            var reservations = _context.Reservations.Where(r => r.Bike == requestedBike);
            foreach(var reservation in reservations)
            {
                _context.Reservations.Remove(reservation);
            }

            _context.Bikes.Remove(requestedBike);// This should auto remove it from Stations bikes list.

            _context.SaveChanges();

            response.Object = requestedBike;
            return response;
        }
    }
}
