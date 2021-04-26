using BikesRentalServer.Authorization;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Services
{
    public class StationsService : IStationsService
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserContext _userContext;

        public StationsService(DatabaseContext context, UserContext userContext)
        {
            _dbContext = context;
            _userContext = userContext;
        }

        public ServiceActionResult<IEnumerable<Station>> GetAllStations()
        {
            var result = _dbContext.Stations.AsEnumerable();
            return ServiceActionResult.Success(result);
        }

        public ServiceActionResult<Station> GetStation(string id)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if(!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            //
            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            return ServiceActionResult.Success(station);
        }
        
        public ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");

            //

            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<IEnumerable<Bike>>("Station not found");
            if(station.Status is BikeStationStatus.Blocked && _userContext.Role is UserRole.User)
                return ServiceActionResult.InvalidState<IEnumerable<Bike>>("User cannot get bikes from blocked station");
            return ServiceActionResult.Success(station.Bikes.AsEnumerable());
        }

        public ServiceActionResult<Station> RemoveStation(string id)
        {
            // TODO: FIX ISSUE WITH TOSTRING
            //

            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            //
            var station = _dbContext.Stations.Include(s => s.Bikes).SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Bikes.Count > 0)
                return ServiceActionResult.InvalidState<Station>("Station has bikes");

            _dbContext.Stations.Remove(station);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(station);
        }
  
        public ServiceActionResult<Station> AddStation(AddStationRequest request)
        {
            var newStation = new Station
            {
                Name = request.Name,
                Status = BikeStationStatus.Working,
            };

            _dbContext.Stations.Add(newStation);
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(newStation);
        }

        public ServiceActionResult<Station> BlockStation(BlockStationRequest request)
        {
            if (!int.TryParse(request.Id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            var station = _dbContext.Stations
                .Include(s => s.Bikes)
                .SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status is BikeStationStatus.Blocked)
                return ServiceActionResult.InvalidState<Station>("Station already blocked");

            station.Status = BikeStationStatus.Blocked;
            _dbContext.SaveChanges();
            return ServiceActionResult.Success(station);
        }

        public ServiceActionResult<Station> UnblockStation(string id)
        {
            if (!int.TryParse(id, out int idAsInt))
                return ServiceActionResult.EntityNotFound<Station>("Station not found");

            var station = _dbContext.Stations
                .SingleOrDefault(s => s.Id == idAsInt);
            if (station is null)
                return ServiceActionResult.EntityNotFound<Station>("Station not found");
            if (station.Status == BikeStationStatus.Working)
                return ServiceActionResult.InvalidState<Station>("Station not blocked");

            station.Status = BikeStationStatus.Working;
            _dbContext.SaveChanges();
            return ServiceActionResult.Success(station);
        }
    }
}
