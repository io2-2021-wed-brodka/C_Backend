using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.ServicesTests.MalfunctionsServiceTests;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.TechsServiceTests
{
    public class RemoveMalfunction : MalfunctionsServiceTestsBase
    {
        [Fact]
        public void RemoveMalfunction_ShouldSucceed()
        {
            const int malfunctionId = 123;
            var bike = new Bike
            {
                Status = BikeStatus.Available,
            };
            var malfunction = new Malfunction
            {
                Id = malfunctionId,
                Bike = bike,
                State = MalfunctionState.NotFixed,
            };
            
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);
            MalfunctionsRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(malfunction);

            var malfunctionsService = GetMalfunctionsService();
            var result = malfunctionsService.RemoveMalfunction(malfunctionId.ToString());

            result.Status.Should().Be(Status.Success);
            MalfunctionsRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(malfunctionId);
        }

        [Fact]
        public void RemoveNotExistingMalfunction_ShouldReturnEntityNotFound()
        {
            const int malfunctionId = 123;
            
            var malfunctionsService = GetMalfunctionsService();
            var result = malfunctionsService.RemoveMalfunction(malfunctionId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            MalfunctionsRepository.Verify();
            result.Object.Should().BeNull();
        }
    }
}
