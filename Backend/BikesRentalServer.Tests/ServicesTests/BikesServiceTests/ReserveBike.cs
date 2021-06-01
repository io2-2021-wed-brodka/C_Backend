using BikesRentalServer.Models;
using BikesRentalServer.Services;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class ReserveBike : BikesServiceTestsBase
    {
        [Fact]
        public void ReserveBikeShouldReturnCreatedReservation()
        {
            const int bikeId = 2;
            var user = new User
            {
                Status = UserStatus.Active,
                Username = "maciek",
            };
            var now = DateTime.Now;
            var reservation = new Reservation
            {
                Bike = new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Reserved,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                },
                User = user,
                ReservationDate = now,
                ExpirationDate = now.AddMinutes(30),
            };
            BikesRepository.Setup(r => r.Get(bikeId.ToString()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                });
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(user);
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), BikeStatus.Reserved))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Reserved,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Returns(reservation).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(reservation);
            ReservationsRepository.Verify();
        }

        [Fact]
        public void ReserveBikeShouldSetBikeStatusToReserved()
        {
            const int bikeId = 2;
            var user = new User
            {
                Status = UserStatus.Active,
                Username = "maciek",
            };
            var reservation = new Reservation
            {
                Bike = new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Reserved,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                },
                User = user,
                ReservationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(30),
            };
            BikesRepository.Setup(r => r.Get(bikeId.ToString()))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Available,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                });
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>())).Returns(user);
            BikesRepository.Setup(r => r.SetStatus(It.IsAny<int>(), BikeStatus.Reserved))
                .Returns(new Bike
                {
                    Id = bikeId,
                    Status = BikeStatus.Reserved,
                    Station = new Station
                    {
                        Status = StationStatus.Active,
                    },
                })
                .Verifiable();
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Returns(reservation);

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.Success);
            BikesRepository.Verify();
        }

        [Fact]
        public void ReserveNotExistingBikeShouldReturnEntityNotFound()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((Bike)null);
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }
        
        [Fact]
        public void ReserveBlockedBikeShouldReturnInvalidState()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Status = BikeStatus.Blocked,
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }
        
        [Fact]
        public void ReserveReservedBikeShouldReturnInvalidState()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Status = BikeStatus.Reserved,
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }
        
        [Fact]
        public void ReserveRentedBikeShouldReturnInvalidState()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Status = BikeStatus.Rented,
                    User = new User(),
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }
        
        [Fact]
        public void ReserveBikeFromBlockedStationShouldReturnInvalidState()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>()))
                .Returns(new Bike
                {
                    Station = new Station
                    {
                        Status = StationStatus.Blocked,
                    },
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.InvalidState);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public void ReserveBikeByBlockedUserShouldReturnUserBlocked()
        {
            const int bikeId = 2;
            BikesRepository.Setup(r => r.Get(It.IsAny<string>())).Returns(new Bike
            {
                Status = BikeStatus.Available,
                Station = new Station
                {
                    Status = StationStatus.Active,
                },
            });
            UsersRepository.Setup(r => r.GetByUsername(It.IsAny<string>()))
                .Returns(new User
                {
                    Status = UserStatus.Blocked,
                });
            ReservationsRepository.Setup(r => r.Add(It.IsAny<Reservation>())).Verifiable();

            var bikesService = GetBikesService();
            var result = bikesService.ReserveBike(bikeId.ToString());

            result.Status.Should().Be(Status.UserBlocked);
            result.Object.Should().BeNull();
            ReservationsRepository.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Never);
        }
    }
}
