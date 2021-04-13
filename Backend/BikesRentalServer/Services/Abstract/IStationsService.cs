﻿using BikesRentalServer.Models;
using System.Collections.Generic;
using BikesRentalServer.Dtos.Requests;

namespace BikesRentalServer.Services.Abstract
{
    public interface IStationsService
    {
        ServiceActionResult<IEnumerable<Station>> GetAllStations();
        ServiceActionResult<Station> GetStation(string id);
        ServiceActionResult<IEnumerable<Bike>> GetAllBikesAtStation(string id);
        ServiceActionResult<Station> RemoveStation(string id);
        ServiceActionResult<Station> AddStation(AddStationRequest request);
    }
}
