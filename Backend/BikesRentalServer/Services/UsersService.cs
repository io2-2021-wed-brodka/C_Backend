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

        public ServiceActionResult<User> BlockUser(string userId)
        {
            var matchingUsers = _dbContext.Users.Where(u => u.Id.ToString() == userId);
            if (matchingUsers.Count() != 1)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");

            var user = matchingUsers.First();
            if (user.State is UserState.Banned)
                return ServiceActionResult.InvalidState<User>("User already blocked");

            user.State = UserState.Banned;

            var userReservations = _dbContext.Reservations.Where(r => r.User.Id.ToString() == userId);
            _dbContext.Reservations.RemoveRange(userReservations);

            // We don't touch user's rented bikes here. He won't be able to rent new ones, he can return only.
            
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<User> UnblockUser(string userId)
        {
            var matchingUsers = _dbContext.Users.Where(u => u.Id.ToString() == userId);
            if (matchingUsers.Count() != 1)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");

            var user = matchingUsers.First();
            if (user.State is UserState.Active)
                return ServiceActionResult.InvalidState<User>("User already unblocked");

            user.State = UserState.Active;
            _dbContext.SaveChanges();

            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<IEnumerable<User>> GetAllUsers()
        {
            var result = _dbContext.Users.Where(u => u.Role == UserRole.User).AsEnumerable();
            return ServiceActionResult.Success(result);
        }
    }
}
