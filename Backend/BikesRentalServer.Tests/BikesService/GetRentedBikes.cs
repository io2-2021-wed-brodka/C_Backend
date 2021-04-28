using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class GetRentedBikes : BikesServiceTestsBase
    {
        public GetRentedBikes() : base()
        {
        }

        [Fact]
        public void GetRentedBikesShouldReturnEmptyIEnumerableWhenNoRentals()
        {
            var noBikes = new List<Bike>();
            BikesRepository.Setup(r => r.GetAll()).Returns(noBikes);

            var bikesService = GetBikesService();

            var result = bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetRentedBikesShouldReturnOnlyRentedBikesOfGivenUser()
        {
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var otherUser = new User
            {
                Id = 1,
                Username = "Mietek"
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
                    Status = BikeStatus.Working, // Rented!!!
                    User = otherUser,
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Working, // Rented !!!
                    User = otherUser,
                },
            };

            var thisUserBikes = new[]
            {
                new Bike
                {
                    Id = 6,
                    Status = BikeStatus.Working, // Rented !!!111
                    User = thisUser,
                },
                new Bike
                {
                    Id = 5,
                    Status = BikeStatus.Working,  // Rented !!!1111oneoneone
                    User = thisUser,
                },
            };
            BikesRepository.Setup(r => r.GetAll())
                .Returns(blockedBikes.Concat(workingBikes).Concat(thisUserBikes).Concat(otherUsersBikes));

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.GetRentedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(thisUserBikes);
        }
    }
}
