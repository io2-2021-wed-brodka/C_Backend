using BikesRentalServer.Authorization;
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
        private readonly DatabaseContext _dbContext;
        private readonly UserContext _userContext;

        public BikesService(DatabaseContext dbContext, UserContext userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public ServiceActionResult<IEnumerable<Bike>> GetAllBikes()
        {
            var result = _dbContext.Bikes.Include(b => b.Station).Include(b => b.User).AsEnumerable();
            return ServiceActionResult.Success(result);
        }

        public ServiceActionResult<Bike> GetBike(string id)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            //
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id == idAsInt);

            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> AddBike(AddBikeRequest request)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(request.StationId, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Station does not exist");

            //
            var station = _dbContext.Stations
                .Include(s => s.Bikes)
                .SingleOrDefault(s => idAsInt == s.Id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Bike>("Station does not exist");

            var newBike = new Bike
            {
                Description = string.Empty,
                Station = station,
            };

            station.Bikes.Add(newBike);
            _dbContext.Bikes.Add(newBike);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(newBike);
        }

        public ServiceActionResult<Bike> RemoveBike(string id)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            //
            var bike = _dbContext.Bikes.SingleOrDefault(b => b.Id == idAsInt);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status != BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");
            if (bike.User is not null)
                throw new InvalidOperationException("Trying to remove rented bike");

            _dbContext.Bikes.Remove(bike);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> RentBike(RentBikeRequest request)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(request.Id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            //
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id == idAsInt);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike is blocked");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Bike>("Bike is already rented");
            if (bike.Station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Station is blocked");

            var user = _dbContext.Users.Single(u => u.Username == _userContext.Username);
            var rentalCount = _dbContext.Bikes.Count(b => b.User.Id == user.Id);
            if (rentalCount >= 4)
                return ServiceActionResult.InvalidState<Bike>("Rental limit exceeded");
            
            var reservation = _dbContext.Reservations.SingleOrDefault(r => r.Bike.Id == bike.Id && r.ExpirationDate > DateTime.Now);
            if (reservation is not null )
            {
                if (reservation.User.Id != user.Id)
                    return ServiceActionResult.InvalidState<Bike>("Bike is reserved by different user");
                
                _dbContext.Reservations.Remove(reservation);
            }

            bike.Station = null;
            bike.User = user;
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<IEnumerable<Bike>> GetRentedBikes()
        {
            var user = _dbContext.Users.Single(u => u.Username == _userContext.Username);
            var bikes = _dbContext.Bikes.Where(b => b.User.Id == user.Id).AsEnumerable();

            return ServiceActionResult.Success(bikes);
        }

        public ServiceActionResult<Bike> GiveBikeBack(string bikeId, string stationId)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(stationId, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Station not found");

            //
            var station = _dbContext.Stations.SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Bike>("Station not found");
            if (station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Station is blocked");
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(bikeId, out int bikeIdAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            //
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id == bikeIdAsInt);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            bike.User = null;
            bike.Station = station;
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> BlockBike(BlockBikeRequest request)
        {
            if (!int.TryParse(request.Id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            
            var bike = _dbContext.Bikes
                .Include(u => u.User)
                .Include(s => s.Station)
                .SingleOrDefault(b => b.Id == idAsInt);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike is already blocked");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Bike>("Bike is rented");
            
            bike.Status = BikeStatus.Blocked;
            _dbContext.SaveChanges();
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> UnblockBike(string id)
        {
            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            var bike = _dbContext.Bikes
                .Include(s => s.Station)
                .SingleOrDefault(b => b.Id == idAsInt);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status == BikeStatus.Working)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");

            bike.Status = BikeStatus.Working;
            _dbContext.SaveChanges();
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes()
        {
            var bikes = _dbContext.Bikes.Where(b => b.Status == BikeStatus.Blocked)
                        .Include(s => s.Station)
                        .AsEnumerable();
            return ServiceActionResult.Success(bikes);
        }
    }
}
