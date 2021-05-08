﻿using BikesRentalServer.DataAccess.Repositories.Abstract;
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

        public UsersService(IUsersRepository usersRepository, IReservationsRepository reservationsRepository)
        {
            _usersRepository = usersRepository;
            _reservationsRepository = reservationsRepository;
        }
        
        #region Basics

        public ServiceActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _usersRepository.GetAll().Where(user => user.Role == UserRole.User);
            return ServiceActionResult.Success(users);
        }
        
        public ServiceActionResult<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _usersRepository.GetByUsernameAndPassword(username, password);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User not found");
            
            return ServiceActionResult.Success(user);
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
            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<string> GenerateBearerToken(User user)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username));
            return ServiceActionResult.Success(token);
        }
        
        #endregion
        
        #region Blocking

        public ServiceActionResult<User> BlockUser(string userId)
        {
            var user = _usersRepository.Get(userId);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");
            if (user.Status is UserStatus.Banned)
                return ServiceActionResult.InvalidState<User>("User already blocked");

            user = _usersRepository.SetStatus(userId, UserStatus.Banned);
            foreach (var reservation in user.Reservations)
                _reservationsRepository.Remove(reservation);

            // We don't touch user's rented bikes here. He won't be able to rent new ones, he can return only.

            return ServiceActionResult.Success(user);
        }

        public ServiceActionResult<User> UnblockUser(string userId)
        {
            var user = _usersRepository.Get(userId);
            if (user is null)
                return ServiceActionResult.EntityNotFound<User>("User doesn't exist");
            if (user.Status is UserStatus.Active)
                return ServiceActionResult.InvalidState<User>("User already unblocked");

            var unblockedUser = _usersRepository.SetStatus(userId, UserStatus.Active);
            return ServiceActionResult.Success(unblockedUser);
        }
        
        #endregion
    }
}
