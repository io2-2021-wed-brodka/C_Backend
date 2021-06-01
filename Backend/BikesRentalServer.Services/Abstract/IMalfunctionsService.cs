using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface IMalfunctionsService
    {
        #region Basics
        ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId);
        ServiceActionResult<Malfunction> AddMalfunction(string bikeId, string description);
        ServiceActionResult<Malfunction> GetMalfunction(string id);

        #endregion
    }
}
