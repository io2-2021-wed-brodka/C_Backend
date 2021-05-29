using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;

namespace BikesRentalServer.Services
{
    public class TechsService : ITechsService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IBikesRepository _bikesRepository;
        private readonly IMalfunctionsRepository _malfunctionsRepository;
        private readonly UserContext _userContext;

        public TechsService(IUsersRepository usersRepository,
                            IBikesRepository bikesRepository,
                            IMalfunctionsRepository malfunctionsRepository,
                            UserContext userContext)
        {
            _usersRepository = usersRepository;
            _bikesRepository = bikesRepository;
            _malfunctionsRepository = malfunctionsRepository;
            _userContext = userContext;
        }

        public ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId)
        {
            var malfunction = _malfunctionsRepository.Get(malfunctionId);
            if (malfunction is null)
                return ServiceActionResult.EntityNotFound<Malfunction>("Malfunction not found");
            
            _malfunctionsRepository.Remove(malfunction.Id);
            return ServiceActionResult.Success(malfunction);
        }
    }
}