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
            return new ServiceActionResult<IEnumerable<Bike>>
            {
                Object = _dbContext.Bikes.Include(b => b.Station).Include(b => b.User),
            };
        }

        public ServiceActionResult<Bike> GetBike(string id)
        {
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id.ToString() == id);

            if (bike is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike not found.",
                    Status = Status.EntityNotFound,
                };
            }
            
            return new ServiceActionResult<Bike>
            {
                Object = bike,
                Status = Status.Success,
            };
        }

        public ServiceActionResult<Bike> AddBike(AddBikeRequest request)
        {
            var station = _dbContext.Stations
                .Include(s => s.Bikes)
                .SingleOrDefault(s => request.StationId == s.Id.ToString());
            if (station is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Station does not exist",
                    Status = Status.EntityNotFound,
                };
            }

            var newBike = new Bike
            {
                Description = string.Empty,
                Station = station,
            };

            station.Bikes.Add(newBike);
            _dbContext.Bikes.Add(newBike);
            _dbContext.SaveChanges();

            return new ServiceActionResult<Bike>
            {
                Object = newBike,
                Status = Status.Success,
            };
        }

        public ServiceActionResult<Bike> RemoveBike(string id)
        {
            var bike = _dbContext.Bikes.SingleOrDefault(b => b.Id.ToString() == id);
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
                    Status = Status.InvalidState,
                };
            }

            if (bike.User is not null)
                throw new InvalidOperationException("Trying to remove rented bike");

            _dbContext.Bikes.Remove(bike);
            _dbContext.SaveChanges();

            return new ServiceActionResult<Bike>
            {
                Object = bike,
                Status = Status.Success,
            };
        }

        public ServiceActionResult<Bike> RentBike(RentBikeRequest request)
        {
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id.ToString() == request.Id);
            if (bike is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike not found.",
                    Status = Status.EntityNotFound,
                };
            }
            if (bike.Status is BikeStatus.Blocked)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike blocked.",
                    Status = Status.InvalidState,
                };
            }
            if (bike.User is not null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike is already rented.",
                    Status = Status.InvalidState,
                };
            }

            var user = _dbContext.Users.Single(u => u.Username == _userContext.Username);
            var rentalCount = _dbContext.Bikes.Count(b => b.User.Id == user.Id);
            if (rentalCount >= 4)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Rental limit exceeded.",
                    Status = Status.InvalidState,
                };
            }
            
            var reservation = _dbContext.Reservations.SingleOrDefault(r => r.Bike.Id == bike.Id && r.ExpiryDate > DateTime.Now);
            if (reservation is not null )
            {
                if (reservation.User.Id != user.Id)
                {
                    return new ServiceActionResult<Bike>
                    {
                        Message = "Bike is reserved by different user.",
                        Status = Status.InvalidState,
                    };
                }
                
                _dbContext.Reservations.Remove(reservation);
            }

            bike.Station = null;
            bike.User = user;
            _dbContext.SaveChanges();

            return new ServiceActionResult<Bike>
            {
                Status = Status.Success,
                Object = bike,
            };
        }

        public ServiceActionResult<IEnumerable<Bike>> GetRentedBikes()
        {
            var user = _dbContext.Users.Single(u => u.Username == _userContext.Username);
            var bikes = _dbContext.Bikes.Where(b => b.User.Id == user.Id);

            return new ServiceActionResult<IEnumerable<Bike>>
            {
                Status = Status.Success,
                Object = bikes,
            };
        }

        public ServiceActionResult<Bike> GiveBikeBack(string bikeId, string stationId)
        {
            var station = _dbContext.Stations.SingleOrDefault(s => s.Id.ToString() == stationId);
            if (station is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Station not found.",
                    Status = Status.EntityNotFound,
                };
            }
            
            var bike = _dbContext.Bikes
                .Include(b => b.User)
                .Include(b => b.Station)
                .SingleOrDefault(b => b.Id.ToString() == bikeId);
            if (bike is null)
            {
                return new ServiceActionResult<Bike>
                {
                    Message = "Bike not found.",
                    Status = Status.EntityNotFound,
                };
            }

            bike.User = null;
            bike.Station = station;
            _dbContext.SaveChanges();

            return new ServiceActionResult<Bike>
            {
                Status = Status.Success,
                Object = bike,
            };
        }
    }
}
