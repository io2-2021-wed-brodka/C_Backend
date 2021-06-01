using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;

namespace BikesRentalServer.Services
{
    public class MalfunctionsService : IMalfunctionsService
    {
        private readonly UserContext _userContext;
        private readonly IMalfunctionsRepository _malfunctionRepository;
        private readonly IBikesRepository _bikesRepository;
        private readonly IUsersRepository _usersRepository;

        public MalfunctionsService(UserContext userContext,
                                   IMalfunctionsRepository malfunctionRepository,
                                   IBikesRepository bikesRepository,
                                   IUsersRepository usersRepository)
        {
            _userContext = userContext;
            _malfunctionRepository = malfunctionRepository;
            _bikesRepository = bikesRepository;
            _usersRepository = usersRepository;
        }

        #region basics
        public ServiceActionResult<Malfunction> AddMalfunction(string bikeId, string description)
        {
            var bike = _bikesRepository.Get(bikeId);
            if (bike is null)
                return ServiceActionResult.EntityNotFound<Malfunction>("Bike not found");
            var user = _usersRepository.GetByUsername(_userContext.Username);

            if (bike.User is null || bike.User.Username != _userContext.Username)
                return ServiceActionResult.InvalidState<Malfunction>("Bike not rented by calling user");

            var malfunction = _malfunctionRepository.Add(new Malfunction
            {
                Bike = bike,
                ReportingUser = user,
                Description = description,
                State = MalfunctionState.NotFixed, 
                DetectionDate = DateTime.Now,
            });
            return ServiceActionResult.Success(malfunction);
        }

        public ServiceActionResult<Malfunction> GetMalfunction(string id)
        {
            var malfunction = _malfunctionRepository.Get(id);
            if (malfunction is null)
                return ServiceActionResult.EntityNotFound<Malfunction>("Malfunction not found");

            return ServiceActionResult.Success(malfunction);
        }

        public ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId)
        {
            var malfunction = _malfunctionRepository.Get(malfunctionId);
            if (malfunction is null)
                return ServiceActionResult.EntityNotFound<Malfunction>("Malfunction not found");
            
            _malfunctionRepository.Remove(malfunction.Id);
            return ServiceActionResult.Success(malfunction);
        }
        #endregion
    }
}
