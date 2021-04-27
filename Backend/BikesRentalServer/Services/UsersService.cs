using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using System;
using System.Collections.Generic;
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
                return ServiceActionResult.InvalidState<User>("Username already taken");

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

            return ServiceActionResult.Success(user);
        }
        
        public ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == Toolbox.ComputeHash(password));
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User not found");
            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<string> GenerateBearerToken(User user)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username));
            return ServiceActionResult.Success(token);
        }

        public ServiceActionResult<IEnumerable<User>> GetAllUsers()
        {
            var result = _dbContext.Users.Where(u => u.Role == UserRole.User).AsEnumerable();
            return ServiceActionResult.Success(result);
        }
    }
}
