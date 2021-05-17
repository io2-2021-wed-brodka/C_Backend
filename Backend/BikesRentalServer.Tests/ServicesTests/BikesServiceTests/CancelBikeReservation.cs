using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class CancelBikeReservation : BikesServiceTestsBase
    {
        [Fact]
        public void CancelBikeReservationShouldSucceed()
        {
            const int bikeId = 123;
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available,
            };

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(bike);
            ReservationsRepository.Setup(r => r.GetActiveReservation(It.IsAny<string>()))
                .Returns(new Reservation
                {
                    Bike = bike,
                });

            var bikesService = GetBikesService();
            var result = bikesService.CancelBikeReservation(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Available);
        }

        [Fact]
        public void CancelBikeReservationOfNotExistingBikeShouldReturnEntityNotFound()
        {
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            ReservationsRepository.Setup(r => r.GetActiveReservation(It.IsAny<string>()))
                .Returns((Reservation)null);
            ReservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.CancelBikeReservation("123");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Remove(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void CancelBikeReservationOfNotReservedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available,
            };

            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(bike);
            ReservationsRepository.Setup(r => r.GetActiveReservation(It.IsAny<string>()))
                .Returns((Reservation)null);
            ReservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.CancelBikeReservation(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Remove(It.IsAny<Reservation>()), Times.Never);
        }
    }
}
