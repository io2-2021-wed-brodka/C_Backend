﻿using BikesRentalServer.Models;
using System.Collections.Generic;

namespace BikesRentalServer.Repositories.Abstract
{
    public interface IBikesRepository : IRepository<Bike>
    {
        IEnumerable<Bike> GetBlocked();
        Bike SetStatus(string id, BikeStatus status);
        Bike Associate(string id, User user);
        Bike Associate(string id, Station station);
    }
}