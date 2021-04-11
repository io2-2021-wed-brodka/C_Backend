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

        public ServiceActionResult<User> AddUser(string username, string password)
        {
            if (_dbContext.Users.Any(u => u.Username == username))
            {
                return new ServiceActionResult<User>
                {
                    Status = Status.InvalidState,
                };
            }

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

            return new ServiceActionResult<User>
            {
                Status = Status.Success,
                Object = user,
            };
        }
        
        public ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == Toolbox.ComputeHash(password));

            if (user is null)
            {
                return new ServiceActionResult<User>
                {
                    Status = Status.EntityNotFound,
                };
            }
            
            return new ServiceActionResult<User>
            {
                Status = Status.Success,
                Object = user,
            };
        }

        public ServiceActionResult<string> GenerateBearerToken(User user)
        {
            return new ServiceActionResult<string>
            {
                Status = Status.Success,
                Object = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username)),
            };
        }
    }
}
