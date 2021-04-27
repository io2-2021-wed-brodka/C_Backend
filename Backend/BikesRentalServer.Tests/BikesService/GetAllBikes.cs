using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.BikesService;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class GetAllBikes : BikesServiceTestsBase
    {
        public GetAllBikes() : base()
        {
        }

        [Fact]
        public void GetAllBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            var noBikes = new List<Bike>();
            BikesRepository.Setup(r => r.GetAll()).Returns(noBikes);

            var bikesService = GetBikesService();

            var result = bikesService.GetAllBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllBikesShouldReturnAllBikes()
        {
            var allBikes = new[]
            {
                new Bike
                {
                    Id = 1,
                    Description = "first one!",
                },
                new Bike
                {
                    Id = 2,
                    Description = "Another ONE",
                },
                new Bike
                {
                    Id = 4,
                    Description = "Skipped one",
                },
                new Bike
                {
                    Id = 7,
                    Description = string.Empty,
                },
            };
            BikesRepository.Setup(r => r.GetAll()).Returns(allBikes);

            var bikesService = GetBikesService();

            var result = bikesService.GetAllBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(allBikes);
        }
    }
}
