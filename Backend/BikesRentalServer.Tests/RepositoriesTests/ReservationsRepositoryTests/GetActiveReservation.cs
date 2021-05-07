using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.ReservationsRepositoryTests
{
    public class GetActiveReservation
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReservationsRepository _reservationsRepository;
        
        public GetActiveReservation()
        {
            _dbContext = MockedDbFactory.GetContext();
            _reservationsRepository = new ReservationsRepository(_dbContext);
        }

        [Fact]
        public void GetActiveReservationShouldReturnActiveReservation()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "bike",
                    Station = new Station
                    {
                        Name = "station",
                        Status = StationStatus.Active,
                    },
                    Status = BikeStatus.Reserved,
                })
                .Entity;
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "user",
                })
                .Entity;
            _dbContext.Reservations.AddRange(new []
            {
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 3, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 3, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 10, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 10, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 17, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 17, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "some bike else",
                    },
                    User = new User
                    {
                        Username = "someone else",
                    },
                    ReservationDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMinutes(30),
                },
            });
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMinutes(30),
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _reservationsRepository.GetActiveReservation(bike.Id.ToString());
            
            result.Should().BeEquivalentTo(reservation);
        }

        [Fact]
        public void GetActiveReservationShouldReturnNullIfNoActiveReservation()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "bike",
                    Station = new Station
                    {
                        Name = "station",
                        Status = StationStatus.Active,
                    },
                    Status = BikeStatus.Reserved,
                })
                .Entity;
            var user = _dbContext.Users.Add(new User
                {
                    Role = UserRole.User,
                    Status = UserStatus.Active,
                    Username = "user",
                })
                .Entity;
            _dbContext.Reservations.AddRange(new []
            {
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 3, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 3, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 10, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 10, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = bike,
                    User = user,
                    ReservationDate = new DateTime(2021, 1, 17, 18, 31, 0),
                    ExpirationDate = new DateTime(2021, 1, 17, 19, 1, 0),
                },
                new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "some bike else",
                    },
                    User = new User
                    {
                        Username = "someone else",
                    },
                    ReservationDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMinutes(30),
                },
            });
            _dbContext.SaveChanges();

            var result = _reservationsRepository.GetActiveReservation(bike.Id.ToString());
            
            result.Should().BeNull();
        }

        [Fact]
        public void GetActiveReservationOfNotExistingBikeShouldReturnNull()
        {
            const string id = "9";

            var result = _reservationsRepository.GetActiveReservation(id);

            result.Should().BeNull();
        }

        [Fact]
        public void GetActiveReservationUsingInvalidIdShouldReturnNull()
        {
            const string id = "invalid";

            var result = _reservationsRepository.GetActiveReservation(id);

            result.Should().BeNull();
        }
    }
}
