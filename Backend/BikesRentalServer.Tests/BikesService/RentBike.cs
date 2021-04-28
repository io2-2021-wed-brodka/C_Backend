using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.BikesService
{
    public class RentBike : BikesServiceTestsBase
    {
        [Fact]
        public void RentBikeShouldSucceed()
        {
            const int bikeId = 123;
            const string username = "zdzislaw";
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available,
                    Station = new Station
                    {
                        Id = 1,
                        Status = StationStatus.Active,
                    },
                });
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<string>(), BikeStatus.Rented)).Verifiable();
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<User>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Rented,
                    User = new User
                    {
                        Username = username,
                    },
                });

            var bikesService = GetBikesService(username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Status.Should().Be(BikeStatus.Rented);
            BikesRepository.Verify();
        }

        [Fact]
        public void RentBikeShouldAssignBikeToUser()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available,
                    Station = new Station
                    {
                        Id = 1,
                    },
                });
            BikesRepository.Setup(r => r.Associate(bikeId.ToString(), It.IsAny<User>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    User = user,
                })
                .Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.User.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RentNotExistingBikeShouldReturnEntityNotFound()
        {
            var thisUser = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };

            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);

            var bikesService = GetBikesService(thisUser.Username);
            var result = bikesService.RentBike("123");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentBlockedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            const int stationId = 1;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Blocked,
                    Station = new Station
                    {
                        Id = stationId,
                        Status = StationStatus.Active,
                    },
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<User>())).Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentAlreadyRentedBikeShouldReturnInvalidState()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = new List<Bike>(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    User = new User
                    {
                        Id = 2,
                        Username = "some other guy",
                    },
                });
            StationsRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Station
                {
                    Id = 1,
                    Status = StationStatus.Active
                });

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.SetStatus(It.IsAny<string>(), It.IsAny<BikeStatus>()), Times.Never);
        }

        [Fact]
        public void RentingBikeAboveLimitShouldReturnInvalidState()
        {
            const int bikeId = 123;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
            };
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    RentedBikes = Enumerable.Range(0, 4).Select(_ => new Bike()).ToList(),
                });
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available,
                    Station = new Station
                    {
                        Id = 1,
                    },
                });
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>())).Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<Station>()), Times.Never);
        }

        [Fact]
        public void RentBikeReservedByRequestingUserShouldSucceed()
        {
            const int bikeId = 123;
            const int stationId = 1;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
                RentedBikes = new List<Bike>(),
            };
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available,
                Station = new Station
                {
                    Id = stationId,
                    Status = StationStatus.Active,
                },
            };

            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(user);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);
            BikesRepository.Setup(r => r.Associate(bikeId.ToString(), It.IsAny<User>()))
                .Returns(new Bike
                {
                    User = user,
                    Id = bikeId,
                })
                .Verifiable();
            ReservationsRepository.Setup(r => r.GetActiveReservation(bikeId.ToString()))
                .Returns(new Reservation
                {
                    Bike = bike,
                    User = user,
                });
            ReservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();
            
            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
            result.Object.Should().NotBeNull();
            result.Object.Id.Should().Be(bikeId);
            result.Object.Station.Should().BeNull();
            result.Object.Status.Should().Be(BikeStatus.Available);
            ReservationsRepository.Verify();
        }

        [Fact]
        public void RentBikeReservedByAnotherUserShouldReturnInvalidState()
        {
            const int bikeId = 123;
            const int stationId = 1;
            var user = new User
            {
                Id = 1,
                Username = "zdzislaw",
                RentedBikes = new List<Bike>(),
            };
            var otherUser = new User
            {
                Id = 3,
                Username = "janusz",
            };
            var station = new Station
            {
                Id = stationId,
                Status = StationStatus.Active,
            };
            var bike = new Bike
            {
                Id = bikeId,
                Status = BikeStatus.Available,
                Station = station,
            };

            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(user);
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);
            BikesRepository.Setup(r => r.Associate(bikeId.ToString(), It.IsAny<User>())).Verifiable();
            ReservationsRepository.Setup(r => r.GetActiveReservation(bikeId.ToString()))
                .Returns(new Reservation
                {
                    Bike = bike,
                    User = otherUser,
                });
            ReservationsRepository.Setup(r => r.Remove(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService(user.Username);
            var result = bikesService.RentBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Remove(It.IsAny<Reservation>()), Times.Never);
            BikesRepository.Verify(r => r.Associate(bikeId.ToString(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void RentBikeFromBlockedStationShouldReturnInvalidState()
        {
            var bike = new Bike
            {
                Id = 123,
                Station = new Station
                {
                    Id = 1,
                    Status = StationStatus.Blocked,
                },
            };
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(bike);
            BikesRepository.Setup(r => r.Associate(It.IsAny<string>(), It.IsAny<User>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.RentBike(bike.Id.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            BikesRepository.Verify(r => r.Associate(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        }
    }
}
