using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.ReservationsRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReservationsRepository _reservationsRepository;
        
        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _reservationsRepository = new ReservationsRepository(_dbContext);
        }

        [Fact]
        public void RemoveUsingIdShouldRemoveReservation()
        {
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            _reservationsRepository.Remove(reservation.Id.ToString());

            _dbContext.Reservations.Count().Should().Be(0);
            _dbContext.Reservations.SingleOrDefault(x => x.Id == reservation.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingEntityShouldRemoveReservation()
        {
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            _reservationsRepository.Remove(reservation);

            _dbContext.Reservations.Count().Should().Be(0);
            _dbContext.Reservations.SingleOrDefault(x => x.Id == reservation.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdShouldReturnRemovedReservation()
        {
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _reservationsRepository.Remove(reservation.Id.ToString());

            result.Should().BeEquivalentTo(reservation);
        }

        [Fact]
        public void RemoveUsingEntityShouldReturnRemovedReservation()
        {
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _reservationsRepository.Remove(reservation);

            result.Should().BeEquivalentTo(reservation);
        }

        [Fact]
        public void RemoveUsingInvalidIdShouldRemoveNothingAndReturnNull()
        {
            const string id = "invalid id";
            _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                });
            _dbContext.SaveChanges();

            var initialReservationCount = _dbContext.Reservations.Count();
            var result = _reservationsRepository.Remove(id);

            _dbContext.Reservations.Count().Should().Be(initialReservationCount);
            result.Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdOfNotExistingReservationShouldRemoveNothingAndReturnNull()
        {
            const string id = "5";
            _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                });
            _dbContext.SaveChanges();

            var initialReservationCount = _dbContext.Reservations.Count();
            var result = _reservationsRepository.Remove(id);

            _dbContext.Reservations.Count().Should().Be(initialReservationCount);
            result.Should().BeNull();
        }

        [Fact]
        public void RemoveNotExistingReservationShouldRemoveNothingAndReturnNull()
        {
            _dbContext.Reservations.Add(new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                        Role = UserRole.User,
                    },
                });
            _dbContext.SaveChanges();

            var initialReservationCount = _dbContext.Reservations.Count();
            var result = _reservationsRepository.Remove(new Reservation
            {
                Id = 6,
            });

            _dbContext.Reservations.Count().Should().Be(initialReservationCount);
            result.Should().BeNull();
        }
    }
}
