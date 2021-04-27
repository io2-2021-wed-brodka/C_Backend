﻿using BikesRentalServer.Models;
using BikesRentalServer.Services;
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
            _bikesRepository.Setup(r => r.GetAll()).Returns(noBikes);

            var bikesService = GetBikesService();

            var result = bikesService.GetBlockedBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetBlocedBikesShouldReturnOnlyBlockedBikes()
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

            var otherBikes = new[]
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
            _bikesRepository.Setup(r => r.GetAll()).Returns(blockedBikes.Concat(otherBikes));

            var bikesService = GetBikesService();

            var result = bikesService.GetAllBikes();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(blockedBikes);
        }
    }
}
