using BikesRentalServer.DataAccess.Repositories.Abstract;
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
        private readonly IUsersRepository _usersRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IBikesRepository _bikesRepository;

        public UsersService(IUsersRepository usersRepository, IReservationsRepository reservationsRepository, IBikesRepository bikesRepository)
        {
            _usersRepository = usersRepository;
            _reservationsRepository = reservationsRepository;
            _bikesRepository = bikesRepository;
        }
        
        #region Basics

        public ServiceActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _usersRepository.GetAll().Where(u => u.Role == UserRole.User);
            return ServiceActionResult.Success(users.Select(user => new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            }));
        }

        public ServiceActionResult<IEnumerable<User>> GetAllTechs()
        {
            var techs = _usersRepository.GetAllTechs().Where(u => u.Role == UserRole.Tech);
            return ServiceActionResult.Success(techs.Select(tech => new User
            {
                Id = tech.Id,
                Reservations = tech.Reservations,
                Role = tech.Role,
                Status = tech.Status,
                Username = tech.Username,
                PasswordHash = tech.PasswordHash,
                RentedBikes = tech.RentedBikes,
            }));
        }

        public ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _usersRepository.GetByUsernameAndPassword(username, password);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User not found");
            
            return ServiceActionResult.Success(new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            });
        }

        public ServiceActionResult<User> AddUser(string username, string password)
        {
            if (_usersRepository.GetByUsername(username) is not null)
                return ServiceActionResult.InvalidState<User>("Username already taken");

            var user = _usersRepository.Add(new User
            {
                Username = username,
                PasswordHash = Toolbox.ComputeHash(password),
                Role = UserRole.User,
                Status = UserStatus.Active,
            });

            return ServiceActionResult.Success(new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            });
        }

        public ServiceActionResult<User> AddTech(string username, string password)
        {
            if (_usersRepository.GetByUsername(username) is not null)
                return ServiceActionResult.InvalidState<User>("Username already taken");

            var user = _usersRepository.Add(new User
            {
                Username = username,
                PasswordHash = Toolbox.ComputeHash(password),
                Role = UserRole.Tech,
                Status = UserStatus.Active,
            });
            return ServiceActionResult.Success(new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            });
        }

        public ServiceActionResult<User> RemoveUser(string id, UserRole supposedRole)
        {
            var user = _usersRepository.Get(id);
            if (user is null || user.Role != supposedRole)
                return ServiceActionResult.EntityNotFound<User>($"{supposedRole} not found");

            user = _usersRepository.Remove(user.Id);
            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<string> GenerateBearerToken(User user)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username));
            return ServiceActionResult.Success(token);
        }

        #endregion

        #region Blocking

        public ServiceActionResult<IEnumerable<User>> GetBlockedUsers()
        {
            var users = _usersRepository.GetBlockedUsers();
            return ServiceActionResult.Success(users.Select(user => new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            }));
        }

        public ServiceActionResult<User> BlockUser(string id)
        {
            var user = _usersRepository.Get(id);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");
            if (user.Status is UserStatus.Blocked)
                return ServiceActionResult.InvalidState<User>("User already blocked");

            var reservations = new Reservation[user.Reservations.Count];
            user.Reservations.CopyTo(reservations);
            foreach (var reservation in reservations)
            {
                _reservationsRepository.Remove(reservation.Id);
                _bikesRepository.SetStatus(reservation.Bike.Id, BikeStatus.Available);
            }
            user = _usersRepository.SetStatus(user.Id, UserStatus.Blocked);

            // We don't touch user's rented bikes here. He won't be able to rent new ones, he can return only.

            return ServiceActionResult.Success(new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            });
        }

        public ServiceActionResult<User> UnblockUser(string id)
        {
            var user = _usersRepository.Get(id);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");
            if (user.Status is UserStatus.Active)
                return ServiceActionResult.InvalidState<User>("User already unblocked");

            user = _usersRepository.SetStatus(user.Id, UserStatus.Active);
            return ServiceActionResult.Success(new User
            {
                Id = user.Id,
                Reservations = user.Reservations,
                Role = user.Role,
                Status = user.Status,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                RentedBikes = user.RentedBikes,
            });
        }
        
        #endregion
    }
}
