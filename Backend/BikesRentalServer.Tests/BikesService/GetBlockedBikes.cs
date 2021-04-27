using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class GetBlockedBikes : BikesServiceTestsBase
    {
        public GetBlockedBikes() : base()
        {
        }

        [Fact]
        public void GetBlocedBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            var noBikes = new List<Bike>();
            BikesRepository.Setup(r => r.GetAll()).Returns(noBikes);

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
