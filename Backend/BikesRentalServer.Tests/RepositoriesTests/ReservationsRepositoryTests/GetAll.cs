using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.ReservationsRepositoryTests
{
    public class GetAll
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReservationsRepository _reservationsRepository;
        
        public GetAll()
        {
            _dbContext = MockedDbFactory.GetContext();
            _reservationsRepository = new ReservationsRepository(_dbContext);
        }

        [Fact]
        public void GetAllShouldReturnAllReservations()
        {
            var reservations = new []
            {
                new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "one",
                    },
                    User = new User
                    {
                        Username = "one",
                    },
                },
                new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "two",
                    },
                    User = new User
                    {
                        Username = "three",
                    },
                },
                new Reservation
                {
                    Bike = new Bike
                    {
                        Description = "four",
                    },
                    User = new User
                    {
                        Username = "two",
                    },
                },
            };
            _dbContext.Reservations.AddRange(reservations);
            _dbContext.SaveChanges();

            var result = _reservationsRepository.GetAll();
            
            result.Should().BeEquivalentTo(reservations);
        }

        [Fact]
        public void GetAllShouldReturnEmptyIEnumerableWhenNoBikes()
        {
            var result = _reservationsRepository.GetAll();
            
            result.Should().BeEmpty();
        }
    }
}
