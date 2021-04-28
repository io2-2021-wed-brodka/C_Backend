﻿using BikesRentalServer.Models;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User GetByUsernameAndPassword(string username, string password);
        User SetStatus(string id, UserStatus status);
    }
}