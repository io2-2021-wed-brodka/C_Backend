using BikesRentalServer.Dtos.Requests;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class RentBike : BikesServiceTestsBase
    {
        public RentBike() : base()
        {
        }
        
        [Fact]
        public void RentBikeShouldSucceed()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.Is<BikeStatus>(s => s == BikeStatus.Working)) // Rented!!!
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    Station = station,
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Working); // Rented
        }

        [Fact]
        public void RentBikeShouldAssignBikeToUser()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()) // Rented!!!
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    Station = station,
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.User.Should().BeEquivalentTo(thisUser);
        }

        [Fact]
        public void RentNotExistingBikeShouldReturnEntityNotFound()
        {
            var bikeId = 123;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()) // Rented!!!
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns((Bike)null);

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentBlockedBikeShouldReturnInvalidState()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                    User = thisUser,
                    Station = station
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentAlreadyRentedBikeShouldReturnInvalidState()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working, //Rented
                    User = thisUser,
                    Station = station
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentingAboveFourthBikeShouldReturnInvalidState()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };

            _bikesRepository.Setup(r =>
                r.Associate(It.IsAny<string>(), It.IsAny<Station>())
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(Enumerable.Range(0, 4).Select(_ => new Bike()));

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Working,
                    User = thisUser,
                });

            _stationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = stationId,
                    Status = StationStatus.Working
                });

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void RentBikeReservedByRequestingUserShouldSucceed()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Working,
                Station = station,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _reservationsRepository.Setup(r => r.GetActiveReservation(bikeId.ToString()))
                .Returns(new Reservation
                {
                    Bike = bike,
                    User = thisUser,
                });

            _reservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(bike);

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.Success);
            _bikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Working); // Rented
            _reservationsRepository.Verify();
        }

        [Fact]
        public void RentBikeReservedByAnotherUserShouldReturnInvalidState()
        {
            var bikeId = 123;
            var stationId = 1;
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw"
            };
            var otherUser = new User
            {
                Id = 3,
                Username = "janusz"
            };
            var rentBikeRequest = new RentBikeRequest
            {
                Id = bikeId.ToString()
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Working,
            };
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Working,
                Station = station,
            };

            _bikesRepository.Setup(r =>
                r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>())
            ).Verifiable();

            _usersRepository.Setup(r =>
                r.GetRentedBikes(It.IsAny<string>())
            ).Returns(new List<Bike>());

            _reservationsRepository.Setup(r => r.GetActiveReservation(bikeId.ToString()))
                .Returns(new Reservation
                {
                    Bike = bike,
                    User = otherUser,
                });

            _reservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();

            _bikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(bike);

            var bikesService = GetBikesService(thisUser.Username);

            var result = bikesService.RentBike(rentBikeRequest);

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            _bikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
            _reservationsRepository.Verify(r => r.Remove(It.IsAny<Reservation>()), Times.Never);
        }
    }
}
