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
        private readonly UserContext _userContext;

        public StationsService(IStationsRepository stationsRepository, UserContext userContext)
        {
            _stationsRepository = stationsRepository;
            _userContext = userContext;
        }

        #region Basics
        
        public ServiceActionResult<IEnumerable<Station>> GetAllStations()
        {
            var stations = _stationsRepository.GetAll();
            return ServiceActionResult.Success(stations);
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            return ServiceActionResult.Success(station);
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");
            if (station.Status is StationStatus.Blocked && _userContext.Role is UserRole.User)
                return ServiceActionResult.InvalidState<IEnumerable<Bike>>("User cannot get bikes from blocked station");

            var bikes = station.Bikes;
            if (_userContext.Role is UserRole.User)
                bikes = bikes.Where(bike => bike.Status == BikeStatus.Available).ToList();

            return ServiceActionResult.Success<IEnumerable<Bike>>(bikes);
        }
  
        public ServiceActionResult<Station> AddStation(string name)
        {
            var station = _stationsRepository.Add(new Station
            {
                Name = name,
                Status = StationStatus.Active,
            });
            return ServiceActionResult.Success(station);
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

            station = _stationsRepository.Remove(id);
            return ServiceActionResult.Success(station);
        }
        
        #endregion
        
        #region Blocking

        public ServiceActionResult<IEnumerable<Station>> GetBlockedStations()
        {
            var stations = _stationsRepository.GetBlocked();
            return ServiceActionResult.Success(stations);
        }

        public ServiceActionResult<IEnumerable<Station>> GetActiveStations()
        {
            var stations = _stationsRepository.GetActive();
            return ServiceActionResult.Success(stations);
        }

        public ServiceActionResult<Station> BlockStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Station>("Station already blocked");

            station = _stationsRepository.SetStatus(id, StationStatus.Blocked);
            return ServiceActionResult.Success(station);
        }

        public ServiceActionResult<Station> UnblockStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status == StationStatus.Active)
                return ServiceActionResult.InvalidState<Station>("Station not blocked");

            station = _stationsRepository.SetStatus(id, StationStatus.Active);
            return ServiceActionResult.Success(station);
        }
        
        #endregion
    }
}
