using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IStationsService
    {
        #region Basics
        
        ServiceActionResult<IEnumerable<Station>> GetAllStations();
        ServiceActionResult<Station> GetStation(string id);
        ServiceActionResult<IEnumerable<Bike>> GetActiveBikesAtStation(string id);
        ServiceActionResult<Station> AddStation(string name, int bikeLimit);
        ServiceActionResult<Station> RemoveStation(string id);
        
        #endregion
        
        #region Blocking
        
        ServiceActionResult<IEnumerable<Station>> GetBlockedStations();
        ServiceActionResult<IEnumerable<Station>> GetActiveStations();
        ServiceActionResult<Station> BlockStation(string id);
        ServiceActionResult<Station> UnblockStation(string id);
        
        #endregion
    }
}
