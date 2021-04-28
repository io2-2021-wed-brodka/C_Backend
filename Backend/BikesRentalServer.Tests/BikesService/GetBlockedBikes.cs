using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetBlockedBikes : BikesServiceTestsBase
    {
        [Fact]
        public void GetBlockedBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            BikesRepository.Setup(r => r.GetAll()).Returns(new List<Bike>());

            var bikesService = GetBikesService();
            var result = bikesService.GetBlockedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }
        
        [Fact]
        public void GetBlockedBikesShouldReturnEmptyIEnumerableIfSomeBikesButNoBlocked()
        {
            BikesRepository.Setup(r => r.GetAll()).Returns(new List<Bike>
            {
                new Bike
                {
                    Id = 2,
                    Status = BikeStatus.Working,
                },
            });

            var bikesService = GetBikesService();
            var result = bikesService.GetBlockedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlockedBikesShouldReturnOnlyBlockedBikes()
        {
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
            BikesRepository.Setup(r => r.GetBlocked()).Returns(blockedBikes);

            var bikesService = GetBikesService();
            var result = bikesService.GetBlockedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(blockedBikes);
        }
    }
}
