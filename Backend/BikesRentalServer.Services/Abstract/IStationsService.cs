using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IStationsService
    {
        ServiceActionResult<IEnumerable<Station>> GetAllStations();
        ServiceActionResult<Station> GetStation(string id);
        ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id);
        ServiceActionResult<Station> RemoveStation(string id);
        ServiceActionResult<Station> AddStation(string name);
        ServiceActionResult<IEnumerable<Station>> GetBlockedStations();
        ServiceActionResult<IEnumerable<Station>> GetActiveStations();
        ServiceActionResult<Station> BlockStation(string id);
        ServiceActionResult<Station> UnblockStation(string id);
    }
}
