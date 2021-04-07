using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IStationsService
    {
        ServiceActionResult<IEnumerable<Station>> GetAllStations();
        ServiceActionResult<Station> GetStation(string id);
        ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id);
    }
}
