﻿using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class GetAllUsers : UsersServiceTestsBase
    {
        [Fact]
        public void GetAllUsersNonShouldExist()
        {
            UsersRepository.Setup(r => r.GetAll()).Returns(new List<User>());

            var usersService = GetUsersService();
            var result = usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllUsersMultipleUsersExists()
        {
            var users = new[]
            {
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "wojtek",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "kasia",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "kacper",
                },
            };
            UsersRepository.Setup(r => r.GetAll()).Returns(users);

            var usersService = GetUsersService();
            var result = usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetAllDontIncludeAdminAndTech()
        {
            var users = new[]
            {
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "wojtek",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "kasia",
                },
                new User
                {
                    Role = UserRole.Admin,
                    Status = UserStatus.Active,
                    Username = "wiesiek",
                },
                new User
                {
                    Role = UserRole.Tech,
                    Status = UserStatus.Active,
                    Username = "maniek",
                },
                new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "kacper",
                },
            };
            UsersRepository.Setup(r => r.GetAll()).Returns(users);

            var usersService = GetUsersService();
            var result = usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().HaveCount(3);
        }
    }
}