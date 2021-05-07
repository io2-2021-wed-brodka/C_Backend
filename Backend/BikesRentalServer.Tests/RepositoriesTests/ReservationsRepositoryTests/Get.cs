using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.ReservationsRepositoryTests
{
    public class Get
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReservationsRepository _reservationsRepository;
        
        public Get()
        {
            _dbContext = MockedDbFactory.GetContext();
            _reservationsRepository = new ReservationsRepository(_dbContext);
        }

        [Fact]
        public void GetShouldReturnRequestedReservationForValidId()
        {
            const int id = 1;
            var reservation = _dbContext.Reservations.Add(new Reservation
                {
                    Id = id,
                    Bike = new Bike
                    {
                        Description = "bike",
                    },
                    User = new User
                    {
                        Username = "user",
                    },
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _reservationsRepository.Get(id.ToString());
            
            result.Should().BeEquivalentTo(reservation);
        }

        [Fact]
        public void GetShouldReturnNullIfNoReservationWithProvidedId()
        {
            const string id = "1";

            var result = _reservationsRepository.Get(id);

            result.Should().BeNull();
        }

        [Fact]
        public void GetShouldReturnNullForInvalidId()
        {
            const string id = "invalid id";

            var result = _reservationsRepository.Get(id);

            result.Should().BeNull();
        }
    }
}
