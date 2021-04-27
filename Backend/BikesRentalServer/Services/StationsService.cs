using BikesRentalServer.Authorization;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using BikesRentalServer.Services.Abstract;
using System.Collections.Generic;

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

            return ServiceActionResult.Success<IEnumerable<Bike>>(station.Bikes);
        }

        public ServiceActionResult<Station> RemoveStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Bikes.Count > 0)
                return ServiceActionResult.InvalidState<Station>("Station has bikes");

            station = _stationsRepository.Remove(id);
            return ServiceActionResult.Success(station);
        }
  
        public ServiceActionResult<Station> AddStation(AddStationRequest request)
        {
            var station = _stationsRepository.Add(new Station
            {
                Name = request.Name,
                Status = StationStatus.Working,
            });
            return ServiceActionResult.Success(station);
        }

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

        public ServiceActionResult<Station> BlockStation(BlockStationRequest request)
        {
            var station = _stationsRepository.Get(request.Id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status is StationStatus.Blocked)
                return ServiceActionResult.InvalidState<Station>("Station already blocked");

            station = _stationsRepository.SetStatus(request.Id, StationStatus.Blocked);
            return ServiceActionResult.Success(station);
        }

        public ServiceActionResult<Station> UnblockStation(string id)
        {
            var station = _stationsRepository.Get(id);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status == StationStatus.Working)
                return ServiceActionResult.InvalidState<Station>("Station not blocked");

            station = _stationsRepository.SetStatus(id, StationStatus.Working);
            return ServiceActionResult.Success(station);
        }
    }
}
