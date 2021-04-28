using BikesRentalServer.Authorization;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
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

        public ServiceActionResult<Bike> AddBike(AddBikeRequest request)
        {
            var station = _stationsRepository.Get(request.StationId);
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

        public ServiceActionResult<Bike> RentBike(RentBikeRequest request)
        {
            var bike = _bikesRepository.Get(request.Id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike is blocked");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Bike>("Bike is already rented");
            if (bike.Station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Station is blocked");

            var user = _usersRepository.GetByUsername(_userContext.Username);
            if (user.RentedBikes.Count >= 4)
                return ServiceActionResult.InvalidState<Bike>("Rental limit exceeded");

            var reservation = _reservationsRepository.GetActiveReservation(request.Id);
            if (reservation is not null )
            {
                if (reservation.User.Id != user.Id)
                    return ServiceActionResult.InvalidState<Bike>("Bike is reserved by different user");

                _reservationsRepository.Remove(reservation);
            }

            bike = _bikesRepository.Associate(request.Id, user);
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<IEnumerable<Bike>> GetRentedBikes()
        {
            var user = _usersRepository.GetByUsername(_userContext.Username);
            return ServiceActionResult.Success<IEnumerable<Bike>>(user.RentedBikes);
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

            bike = _bikesRepository.Associate(bikeId, station);
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> BlockBike(BlockBikeRequest request)
        {
            var bike = _bikesRepository.Get(request.Id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status is BikeStatus.Blocked)
                return ServiceActionResult.InvalidState<Bike>("Bike is already blocked");
            if (bike.User is not null)
                return ServiceActionResult.InvalidState<Bike>("Bike is rented");

            bike = _bikesRepository.SetStatus(request.Id, BikeStatus.Blocked);
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<Bike> UnblockBike(string id)
        {
            var bike = _bikesRepository.Get(id);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Bike>("Bike not found");
            if (bike.Status == BikeStatus.Working)
                return ServiceActionResult.InvalidState<Bike>("Bike not blocked");

            bike = _bikesRepository.SetStatus(id, BikeStatus.Working);
            return ServiceActionResult.Success(bike);
        }

        public ServiceActionResult<IEnumerable<Bike>> GetBlockedBikes()
        {
            var bikes = _bikesRepository.GetBlocked();
            return ServiceActionResult.Success(bikes);
        }
    }
}
