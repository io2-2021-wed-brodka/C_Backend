using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return ServiceActionResult.Success(bikes.Select(bike =>
            {
                var reservation = _reservationsRepository.GetActiveReservation(bike.Id);
                return new Bike
                {
                    Description = bike.Description,
                    Id = bike.Id,
                    Station = bike.Station,
                    Status = reservation is null ? bike.Status : BikeStatus.Reserved,
                    User = reservation is null ? bike.User : reservation.User,
                    StationId = bike.StationId,
                    Malfunctions = bike.Malfunctions,
                };
            }));
        }

        public ServiceActionResult<Bike> GetBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            var reservation = _reservationsRepository.GetActiveReservation(bike.Id);
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = reservation is null ? bike.Status : BikeStatus.Reserved,
                User = reservation is null ? bike.User : reservation.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
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
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = _reservationsRepository.GetActiveReservation(bike.Id) is null ? bike.Status : BikeStatus.Reserved,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
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

            bike = _bikesRepository.Remove(bike.Id);
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = _reservationsRepository.GetActiveReservation(bike.Id) is null ? bike.Status : BikeStatus.Reserved,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
        }
        
        #endregion
        
        #region Renting

        public ServiceActionResult<IEnumerable<Bike>> GetRentedBikes()
        {
            var user = _usersRepository.GetByUsername(_userContext.Username);
            return ServiceActionResult.Success(user.RentedBikes.Select(bike => new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            }));
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

            var reservation = _reservationsRepository.GetActiveReservation(bike.Id);
            if (reservation is not null )
            {
                if (reservation.User.Id != user.Id)
                    return ServiceActionResult.InvalidState<Bike>("Bike is reserved by different user");

                _reservationsRepository.Remove(reservation.Id);
            }

            _bikesRepository.SetStatus(bike.Id, BikeStatus.Rented);
            bike = _bikesRepository.AssociateWithUser(bike.Id, user.Id);
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
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

            _bikesRepository.SetStatus(bike.Id, BikeStatus.Available);
            bike = _bikesRepository.AssociateWithStation(bike.Id, station.Id);
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
        }
        
        #endregion
        
        #region Blocking

        public ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes()
        {
            var bikes = _bikesRepository.GetBlocked();
            return ServiceActionResult.Success(bikes.Select(bike => new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            }));
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
                    bike = _bikesRepository.SetStatus(bike.Id, BikeStatus.Blocked);
                    var reservation = _reservationsRepository.GetActiveReservation(bike.Id);
                    if (reservation is not null)
                        _reservationsRepository.Remove(reservation.Id);
                    return ServiceActionResult.Success(new Bike
                    {
                        Description = bike.Description,
                        Id = bike.Id,
                        Station = bike.Station,
                        Status = bike.Status,
                        User = bike.User,
                        StationId = bike.StationId,
                        Malfunctions = bike.Malfunctions,
                    });
            }
        }

        public ServiceActionResult<Bike> UnblockBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Available)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");

            bike = _bikesRepository.SetStatus(bike.Id, BikeStatus.Available);
            return ServiceActionResult.Success(new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
        }
        
        #endregion
        
        #region Reserving

        public ServiceActionResult<IEnumerable<Bike>> GetReservedBikes()
        {
            var user = _usersRepository.GetByUsername(_userContext.Username);
            var reservations = _reservationsRepository.GetActiveReservations(user.Id);
            var reservedBikes = reservations.Select(reservation => reservation.Bike);
            return ServiceActionResult.Success(reservedBikes.Select(bike => new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = BikeStatus.Reserved,
                User = bike.User,
                StationId = bike.StationId,
                Malfunctions = bike.Malfunctions,
            }));
        }

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
            if (_reservationsRepository.GetActiveReservation(bike.Id) is not null)
                return ServiceActionResult.InvalidState<Reservation>("Reservation for bike exists");

            var user = _usersRepository.GetByUsername(_userContext.Username);
            if (user.Status is UserStatus.Blocked)
                return ServiceActionResult.UserBlocked<Reservation>("User is blocked");
            if (user.Reservations.Count >= 3)
                return ServiceActionResult.InvalidState<Reservation>("Reservation limit exceeded");

            bike = _bikesRepository.SetStatus(bike.Id, BikeStatus.Reserved);
            var reservation = _reservationsRepository.Add(new Reservation
            {
                User = user,
                Bike = bike,
                ReservationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(30),
            });
            return ServiceActionResult.Success(new Reservation
            {
                Bike = reservation.Bike,
                Id = reservation.Id,
                User = reservation.User,
                ExpirationDate = reservation.ExpirationDate,
                ReservationDate = reservation.ReservationDate,
            });
        }

        public ServiceActionResult<Bike> CancelBikeReservation(string bikeId)
        {
            var bike = _bikesRepository.Get(bikeId);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");

            var reservation = _reservationsRepository.GetActiveReservation(bike.Id);
            if (reservation is null)
                return ServiceActionResult.InvalidState<Bike>("Bike is not reserved");

            _bikesRepository.SetStatus(bike.Id, BikeStatus.Available);
            _reservationsRepository.Remove(reservation.Id);
            return ServiceActionResult.Success(new Bike
            {
                Description = reservation.Bike.Description,
                Id = reservation.Bike.Id,
                Station = reservation.Bike.Station,
                Status = reservation.Bike.Status,
                User = reservation.Bike.User,
                StationId = reservation.Bike.StationId,
                Malfunctions = bike.Malfunctions,
            });
        }

        #endregion
    }
}
