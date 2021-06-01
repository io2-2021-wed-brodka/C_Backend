using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;

namespace BikesRentalServer.Services
{
    public class MalfunctionsService : IMalfunctionsService
    {
        private readonly IMalfunctionsRepository _malfunctionsRepository;

        public MalfunctionsService(IMalfunctionsRepository malfunctionsRepository)
        {
            _malfunctionsRepository = malfunctionsRepository;
        }

        #region Basic

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
