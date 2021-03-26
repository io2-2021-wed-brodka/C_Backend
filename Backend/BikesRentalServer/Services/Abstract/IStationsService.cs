using System.Collections.Generic;
using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Dtos.Responses;
using BikesRentalServer.Models;

namespace BikesRentalServer.Services
{
    public interface IStationsService
    {
        IEnumerable<Station> GetAllStations();
        Station GetStation(string id);
        IEnumerable<Bike> GetAllBikesAtStation(int id);
    }
}
