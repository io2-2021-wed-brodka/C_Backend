using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class GetAllBikes : BikesServiceTestsBase
    {
        [Fact]
        public void GetAllBikesShouldReturnEmptyIEnumerableIfNoBikes()
        {
            BikesRepository.Setup(r => r.GetAll()).Returns(new List<Bike>());

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
