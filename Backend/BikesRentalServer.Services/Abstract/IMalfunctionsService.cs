using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IMalfunctionsService
    {
        #region Basics

        ServiceActionResult<IEnumerable<Malfunction>> GetAllMalfunctions();

        ServiceActionResult<Malfunction> GetMalfunction(string id);
        ServiceActionResult<Malfunction> AddMalfunction(string bikeId, string description);

        #endregion
    }
}
