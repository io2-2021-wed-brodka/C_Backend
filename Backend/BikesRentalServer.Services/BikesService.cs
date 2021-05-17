using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;
using System.Collections.Generic;

namespace BikesRentalServer.Services
{
    public class BikesService : IBikesService
    {
        private readonly IBikesRepository _bikesRepository;
        private readonly IStationsRepository _stationsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly UserContext _userContext;

        public BikesService(IBikesRepository bikesRepository,
                            IStationsRepository stationsRepository,
                            IUsersRepository usersRepository,
                            IReservationsRepository reservationsRepository,
                            UserContext userContext)
        {
            _bikesRepository = bikesRepository;
            _stationsRepository = stationsRepository;
            _usersRepository = usersRepository;
            _reservationsRepository = reservationsRepository;
            _userContext = userContext;
        }

        #region Basics
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikes()
        {
            var bikes = _bikesRepository.GetAll();
            return ServiceActionResult.Success(bikes);
        }

        public ServiceActionResult<Bike> GetBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> AddBike(string stationId)
        {
            var station = _stationsRepository.Get(stationId);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Bike>("Station does not exist");

            var bike = _bikesRepository.Add(new Bike
            {
                Station = station,
                Status = Bike.DefaultBikeStatus,
            });
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> RemoveBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is not BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");
            if (bike.User is not null)
                throw new InvalidOperationException("Trying to remove rented bike");

            bike = _bikesRepository.Remove(bike);
            return ServiceActionResult.Success(bike);
        }
        
        #endregion
        
        #region Renting

        public ServiceActionResult<IEnumerable<Bike>> GetRentedBikes()
        {
            var user = _usersRepository.GetByUsername(_userContext.Username);
            return ServiceActionResult.Success<IEnumerable<Bike>>(user.RentedBikes);
        }

        public ServiceActionResult<Bike> RentBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike is blocked");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Bike>("Bike is already rented");
            if (bike.Station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Station is blocked");

            var user = _usersRepository.GetByUsername(_userContext.Username);
            if (user.Status is UserStatus.Blocked)
                return ServiceActionResult.UserBlocked<Bike>("User is blocked");
            if (user.RentedBikes.Count >= 4)
                return ServiceActionResult.InvalidState<Bike>("Rental limit exceeded");

            var reservation = _reservationsRepository.GetActiveReservation(id);
            if (reservation is not null )
            {
                if (reservation.User.Id != user.Id)
                    return ServiceActionResult.InvalidState<Bike>("Bike is reserved by different user");

                _reservationsRepository.Remove(reservation);
            }

            _bikesRepository.SetStatus(id, BikeStatus.Rented);
            bike = _bikesRepository.Associate(id, user);
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> GiveBikeBack(string bikeId, string stationId)
        {
            var station = _stationsRepository.Get(stationId);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Bike>("Station not found");
            if (station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Station is blocked");

            var bike = _bikesRepository.Get(bikeId);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.User.Username != _userContext.Username)
                return ServiceActionResult.InvalidState<Bike>("Bike not rented by calling user");

            _bikesRepository.SetStatus(bikeId, BikeStatus.Available);
            bike = _bikesRepository.Associate(bikeId, station);
            return ServiceActionResult.Success(bike);
        }
        
        #endregion
        
        #region Blocking

        public ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes()
        {
            var bikes = _bikesRepository.GetBlocked();
            return ServiceActionResult.Success(bikes);
        }

        public ServiceActionResult<Bike> BlockBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            switch (bike.Status)
            {
                case BikeStatus.Blocked:
                    return ServiceActionResult.InvalidState<Bike>("Bike is already blocked");
                case BikeStatus.Rented:
                    return ServiceActionResult.InvalidState<Bike>("Bike is rented");
                default:
                    bike = _bikesRepository.SetStatus(id, BikeStatus.Blocked);
                    return ServiceActionResult.Success(bike);
            }
        }

        public ServiceActionResult<Bike> UnblockBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Available)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");

            bike = _bikesRepository.SetStatus(id, BikeStatus.Available);
            return ServiceActionResult.Success(bike);
        }
        
        #endregion
        
        #region Reserving

        public ServiceActionResult<IEnumerable<Bike>> GetReservedBikes() => throw new NotImplementedException();

        public ServiceActionResult<Reservation> ReserveBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Reservation>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Reservation>("Bike is blocked");
            if (bike.Status is BikeStatus.Reserved)
                return ServiceActionResult.InvalidState<Reservation>("Bike is reserved");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Reservation>("Bike is rented");
            if (bike.Station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Reservation>("Station is blocked");

            var user = _usersRepository.GetByUsername(_userContext.Username);
            if (user.Status is UserStatus.Blocked)
                return ServiceActionResult.UserBlocked<Reservation>("User is blocked");

            _bikesRepository.SetStatus(id, BikeStatus.Reserved);
            var reservation = _reservationsRepository.Add(new Reservation
            {
                User = user,
                Bike = bike,
                ReservationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(30),
            });
            return ServiceActionResult.Success(reservation);
        }

        public ServiceActionResult<Bike> CancelBikeReservation(string id) => throw new NotImplementedException();

        #endregion
    }
}
