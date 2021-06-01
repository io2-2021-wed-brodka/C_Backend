using System;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.MalfunctionsServiceTests
{
    public class AddMalfunction : MalfunctionsServiceTestsBase
    {
        [Fact]
        public void AddMalfunctionShouldReturnMalfunction()
        {
            const int bikeId = 2;
            const string description = "malfunction description";
            var user = new User()
            {
                Status = UserStatus.Active,
                Username = "maciek",
            };
            var malfunction = new Malfunction
            {
                Bike = new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Reserved,
                    Station = null,
                    User = user
                },
                DetectionDate = DateTime.Now,
                ReportingUser = user,
                Description = description,
                State = MalfunctionState.NotFixed
            };
            BikesRepository.Setup(r => r.Get(bikeId.ToString()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Rented,
                    User = user,
                });
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(user);
            MalfunctionsRepository.Setup(r => r.Add(It.IsAny<Malfunction>())).Returns(malfunction).Verifiable();
            var malfunctionService = GetMalfunctionsService(user.Username, user.Role);
            var result = malfunctionService.AddMalfunction(bikeId.ToString(), description);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().Be(malfunction);
            MalfunctionsRepository.Verify();
        }

        [Fact]
        public void AddMalfunctionToNotExistingBikeShouldReturnEntityNotFound()
        {
            const int bikeId = 2;
            const string description = "some description";
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            MalfunctionsRepository.Setup(r => r.Add(It.IsAny<Malfunction>())).Verifiable();

            var malfunctionsService = GetMalfunctionsService();
            var result = malfunctionsService.AddMalfunction(bikeId.ToString(), description);
            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            MalfunctionsRepository.Verify(r => r.Add(It.IsAny<Malfunction>()), Times.Never);

        }

        [Fact]
        public void AddMalfunctionToBikeNotRentedByUserShouldReturnInvalidState()
        {
            const int bikeId = 123;
            const string description = "some description";
            var user = new User
            {
                Id = 1,
                Username = "Mariusz"
            };
            var otherUser = new User
            {
                Id = 2,
                Username = "Pudzianowski"
            };
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    User = otherUser,
                });
            var malfunctionService = GetMalfunctionsService(user.Username);
            var result = malfunctionService.AddMalfunction(bikeId.ToString(), description);
            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            MalfunctionsRepository.Verify(r => r.Add(It.IsAny<Malfunction>()), Times.Never);
        }
    }
}
