using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Services
{
    public class StationsService : IStationsService
    {
        private readonly IStationsRepository _stationsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly UserContext _userContext;

        public StationsService(IStationsRepository stationsRepository, IReservationsRepository reservationsRepository, UserContext userContext)
        {
            _stationsRepository = stationsRepository;
            _reservationsRepository = reservationsRepository;
            _userContext = userContext;
        }

        #region Basics
        
        public ServiceActionResult<IEnumerable<Station>> GetAllStations()
        {
            var stations = _stationsRepository.GetAll();
            return ServiceActionResult.Success(stations.Select(station => new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            }));
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            return ServiceActionResult.Success(new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            } );
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetActiveBikesAtStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");
            if (station.Status is StationStatus.Blocked && _userContext.Role is UserRole.User)
                return ServiceActionResult.InvalidState<IEnumerable<Bike>>("User cannot get bikes from blocked station");

            var bikes = station.Bikes.Where(bike => bike.Status is BikeStatus.Available && _reservationsRepository.GetActiveReservation(bike.Id) is null);
            return ServiceActionResult.Success(bikes.Select(bike => new Bike
            {
                Description = bike.Description,
                Id = bike.Id,
                Station = bike.Station,
                Status = bike.Status,
                User = bike.User,
                StationId = bike.StationId,
            }));
        }
  
        public ServiceActionResult<Station> AddStation(string name, int bikeLimit)
        {
            if (bikeLimit <= 0)
                return ServiceActionResult.InvalidState<Station>("Invalid bike limit");
            
            var station = _stationsRepository.Add(new Station
            {
                Name = name,
                Status = StationStatus.Active,
                BikeLimit = bikeLimit,
            });
            return ServiceActionResult.Success(new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            });
        }

        public ServiceActionResult<Station> RemoveStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status is StationStatus.Active)
                return ServiceActionResult.InvalidState<Station>("Station is active");
            if (station.Bikes.Count > 0)
                return ServiceActionResult.InvalidState<Station>("Station has bikes");

            station = _stationsRepository.Remove(station.Id);
            return ServiceActionResult.Success(new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            });
        }
        
        #endregion
        
        #region Blocking

        public ServiceActionResult<IEnumerable<Station>> GetBlockedStations()
        {
            var stations = _stationsRepository.GetBlocked();
            return ServiceActionResult.Success(stations.Select(station => new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            }));
        }

        public ServiceActionResult<IEnumerable<Station>> GetActiveStations()
        {
            var stations = _stationsRepository.GetActive();
            return ServiceActionResult.Success(stations.Select(station => new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            }));
        }

        public ServiceActionResult<Station> BlockStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Station>("Station already blocked");

            station = _stationsRepository.SetStatus(station.Id, StationStatus.Blocked);
            return ServiceActionResult.Success(new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            });
        }

        public ServiceActionResult<Station> UnblockStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status == StationStatus.Active)
                return ServiceActionResult.InvalidState<Station>("Station not blocked");

            station = _stationsRepository.SetStatus(station.Id, StationStatus.Active);
            return ServiceActionResult.Success(new Station
            {
                Bikes = station.Bikes,
                Id = station.Id,
                Name = station.Name,
                Status = station.Status,
                BikeLimit = station.BikeLimit,
            });
        }
        
        #endregion
    }
}
