using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetRentedBikes : BikesServiceTestsBase
    {
        [Fact]
        public void GetRentedBikesShouldReturnEmptyIEnumerableWhenNoRentals()
        {
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });

            var bikesService = GetBikesService();
            var result = bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetRentedBikesShouldReturnOnlyRentedBikesOfGivenUser()
        {
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            var otherUser = new User
            {
                Id = 1,
                Username = "Mietek",
            };
            var blockedBikes = new[]
            {
                new Bike
                {
                    Id = 1,
                    Status = BikeStatus.Blocked,
                },
                new Bike
                {
                    Id = 2,
                    Status = BikeStatus.Blocked,
                },
            };
            var workingBikes = new[]
            {
                new Bike
                {
                    Id = 6,
                    Status = BikeStatus.Working
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Working
                },
            };
            var otherUsersBikes = new[]
            {
                new Bike
                {
                    Id = 6,
                    Status = BikeStatus.Working,
                    User = otherUser,
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Working,
                    User = otherUser,
                },
            };
            var thisUsersBikes = new[]
            {
                new Bike
                {
                    Id = 6,
                    Status = BikeStatus.Working,
                    User = user,
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Working,
                    User = user,
                },
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = thisUsersBikes.ToList(),
                });

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(thisUsersBikes);
        }
    }
}
