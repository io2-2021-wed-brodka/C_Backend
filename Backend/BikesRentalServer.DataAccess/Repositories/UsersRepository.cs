using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext _dbContext;
        
        public UsersRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users.Include(u => u.RentedBikes).Include(u => u.Reservations).Where(u => u.Role == UserRole.User);
        }

        public User Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Users.Include(u => u.RentedBikes).Include(u => u.Reservations).SingleOrDefault(u => u.Id == iid && u.Role == UserRole.User);
        }

        public User Add(User entity)
        {
            var user = _dbContext.Users.Add(entity).Entity;
            _dbContext.SaveChanges();

            return user;
        }

        public User Remove(string id)
        {
            var user = Get(id);
            if (user is null)
                return null;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return user;
        }

        public User Remove(User entity)
        {
            if (!_dbContext.Users.Contains(entity))
                return null;
            
            var user = _dbContext.Users.Remove(entity).Entity;
            _dbContext.SaveChanges();

            return user;
        }

        public IEnumerable<User> GetBlockedUsers()
        {
            return _dbContext.Users.Include(u => u.RentedBikes).Include(u => u.Reservations).Where(u => u.Role == UserRole.User && u.Status == UserStatus.Blocked);
        }

        public User GetByUsername(string username)
        {
            return _dbContext.Users.Include(u => u.RentedBikes).Include(u => u.Reservations).SingleOrDefault(u => u.Username == username);
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return _dbContext.Users
                .Include(u => u.RentedBikes)
                .Include(u => u.Reservations)
                .SingleOrDefault(u => u.Username == username && u.PasswordHash == Toolbox.ComputeHash(password));
        }

        public User SetStatus(string id, UserStatus status)
        {
            var user = Get(id);
            if (user is null)
                return null;

            user.Status = status;
            _dbContext.SaveChanges();

            return user;
        }
    }
}
