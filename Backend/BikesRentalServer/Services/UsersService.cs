using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;
using System.Linq;
using System.Text;

namespace BikesRentalServer.Services
{
    public class UsersService : IUsersService
    {
        private readonly DatabaseContext _dbContext;

        public UsersService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddUser(string username, string password)
        {
            if (_dbContext.Users.Any(u => u.Username == username))
                return null;

            var user = _dbContext.Users
                .Add(new User
                {
                    Username = username,
                    PasswordHash = Toolbox.ComputeHash(password),
                    Role = UserRole.User,
                    State = UserState.Active,
                })
                .Entity;
            _dbContext.SaveChanges();

            return user;
        }
        
        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == Toolbox.ComputeHash(password));
        }

        public string GenerateBearerToken(User user)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username));
        }
    }
}
