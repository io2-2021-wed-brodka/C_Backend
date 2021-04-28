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
            var usersBikes = new[]
            {
                new Bike
                {
                    Id = 6,
                    Status = BikeStatus.Available,
                    User = user,
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Available,
                    User = user,
                },
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = usersBikes.ToList(),
                });

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(usersBikes);
        }
    }
}
