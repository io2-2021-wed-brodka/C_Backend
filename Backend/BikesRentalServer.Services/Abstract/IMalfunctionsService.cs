using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IMalfunctionsService
    {
        #region Basics
        
        ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId);
        ServiceActionResult<Malfunction> GetMalfunction(string id);
        ServiceActionResult<IEnumerable<Malfunction>> GetAllMalfunctions();
        ServiceActionResult<Malfunction> AddMalfunction(string bikeId, string description);

        #endregion
    }
}
