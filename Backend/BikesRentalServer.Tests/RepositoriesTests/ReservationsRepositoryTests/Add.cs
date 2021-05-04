using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.ReservationsRepositoryTests
{
    public class Add
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReservationsRepository _reservationsRepository;
        
        public Add()
        {
            _dbContext = MockedDbFactory.GetContext();
            _reservationsRepository = new ReservationsRepository(_dbContext);
        }

        [Fact]
        public void AddShouldAddReservationToDatabase()
        {
            _reservationsRepository.Add(new Reservation());

            _dbContext.Reservations.Count().Should().Be(1);
        }

        [Fact]
        public void AddShouldReturnAddedReservation()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                    Status = UserStatus.Active,
                    Role = UserRole.Tech,
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Description = "bike",
                    Station = new Station
                    {
                        Name = "station",
                    },
                    Status = BikeStatus.Reserved,
                })
                .Entity;
            _dbContext.SaveChanges();
            
            var result = _reservationsRepository.Add(new Reservation
            {
                Bike = bike,
                User = user,
            });

            result.Should().NotBeNull();
            result.Id.Should().BePositive();
            result.Bike.Should().BeEquivalentTo(bike);
            result.User.Should().BeEquivalentTo(user);
        }
    }
}
