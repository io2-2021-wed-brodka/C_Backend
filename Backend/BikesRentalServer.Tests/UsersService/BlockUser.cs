using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.UsersService
{
    public class BlockUser
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;

        public BlockUser()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void BlockUserThatDoesntExist()
        {
            var response = _usersService.BlockUser("");

            response.Status.Should().Be(Status.EntityNotFound);
            response.Object.Should().Be(null);
        }

        [Fact]
        public void BlockUserAlreadyBlocked()
        {
            int testId = 100;
            _dbContext.Users.Add(new User
            {
                Id = testId,
                State = UserState.Banned,
            });
            _dbContext.SaveChanges();
            var response = _usersService.BlockUser(testId.ToString());

            response.Status.Should().Be(Status.InvalidState);
            response.Object.Should().Be(null);
        }

        [Fact]
        public void BlockUserSimpleSuccess()
        {
            int testId = 100;
            _dbContext.Users.Add(new User
            {
                Id = testId,
                State = UserState.Active,
            });
            _dbContext.SaveChanges();
            var response = _usersService.BlockUser(testId.ToString());

            response.Status.Should().Be(Status.Success);
            response.Object.Id.Should().Be(testId);
        }

        [Fact]
        public void BlockUserWithReservations()
        {
            int testId = 100;
            var user = new User();
            user.Id = testId;
            user.State = UserState.Active;

            _dbContext.Users.Add(user);

            var bike1 = new Bike();
            var bike2 = new Bike();
            var bike3 = new Bike();
            
            _dbContext.Bikes.Add(bike1);
            _dbContext.Bikes.Add(bike2);
            _dbContext.Bikes.Add(bike3);

            _dbContext.Reservations.Add(new Reservation()
            {
                Bike = bike1,
                User = user,
            });

            _dbContext.Reservations.Add(new Reservation()
            {
                Bike = bike2,
                User = user,
            });
            _dbContext.Reservations.Add(new Reservation()
            {
                Bike = bike3,
            });
            _dbContext.SaveChanges();
            _dbContext.Reservations.Should().HaveCount(3);

            _dbContext.SaveChanges();
            var response = _usersService.BlockUser(testId.ToString());
            _dbContext.SaveChanges();

            response.Status.Should().Be(Status.Success);
            response.Object.Id.Should().Be(testId);
            _dbContext.Reservations.Should().HaveCount(1);
        }
    }
}
