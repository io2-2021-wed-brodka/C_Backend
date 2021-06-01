using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;

namespace BikesRentalServer.Services
{
    public class TechsService : ITechsService
    {
        private readonly IMalfunctionsRepository _malfunctionsRepository;

        public TechsService(IMalfunctionsRepository malfunctionsRepository)
        {
            _malfunctionsRepository = malfunctionsRepository;
        }

        #region Malfunctions

        public ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId)
        {
            var malfunction = _malfunctionsRepository.Get(malfunctionId);
            if (malfunction is null)
                return ServiceActionResult.EntityNotFound<Malfunction>("Malfunction not found");
            
            _malfunctionsRepository.Remove(malfunction.Id);
            return ServiceActionResult.Success(malfunction);
        }

        #endregion
    }
}
