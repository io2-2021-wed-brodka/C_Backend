using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Services.Abstract
{
    public interface IStationsService
    {
        IEnumerable<Station> GetAllStations();
        Station GetStation(string id);
        IEnumerable<Bike> GetAllBikesAtStation(string id);
    }
}
