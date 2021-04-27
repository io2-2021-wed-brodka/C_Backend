using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Repositories
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
            return _dbContext.Users;
        }

        public User Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Users.SingleOrDefault(u => u.Id == iid);
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
            var user = _dbContext.Users.Remove(entity).Entity;
            _dbContext.SaveChanges();

            return user;
        }

        public User GetByUsername(string username)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Username == username);
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

        public IEnumerable<Bike> GetRentedBikes(string id)
        {
            var user = Get(id);
            return user?.RentedBikes;
        }
    }
}
