﻿using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class BlockUser : UsersServiceTestsBase
    {
        public BlockUser() : base()
        {
        }

        [Fact]
        public void BlockNotExistingUserShouldReturnEntityNotFound()
        {
            var userId = "2";
            _usersRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((User)null);
            _reservationsRepository.Setup(r => r.GetAll()).Returns(new List<Reservation>());

            var usersService = GetUsersService();

            var response = usersService.BlockUser(userId);

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockAlreadyBlockedUserShouldReturnInvalidState()
        {
            var userId = "2";
            _reservationsRepository.Setup(r => r.GetAll()).Returns(new List<Reservation>());
            _usersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = 34,
                    Status = UserStatus.Banned
                });

            var usersService = GetUsersService();

            var response = usersService.BlockUser(userId);

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().BeNull();
        }

        [Fact]
        public void BlockUserShouldSetUserStateToBanned()
        {
            var userId = "2";
            _reservationsRepository.Setup(r => r.GetAll()).Returns(new List<Reservation>());
            _usersRepository.Setup(r => r.SetStatus(It.IsAny<string>(), It.Is<UserStatus>(s => s == UserStatus.Banned)))
                .Returns(new User
                {
                    Id = 2,
                    Status = UserStatus.Banned,
                    Reservations = new List<Reservation>()
                }).Verifiable();
            _usersRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new User
                {
                    Id = 2,
                    Status = UserStatus.Active,
                    Reservations = new List<Reservation>()
                });

            var usersService = GetUsersService();

            var response = usersService.BlockUser(userId);

            response.Status.Should().Be(Status.Success);
            response.Object.Should().NotBeNull();
            response.Object.Status.Should().Be(UserStatus.Banned);
            _usersRepository.Verify();
        }
    }
}
