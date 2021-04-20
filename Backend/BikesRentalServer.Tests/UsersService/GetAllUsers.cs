using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Services;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;


namespace BikesRentalServer.Tests.UsersService
{
    public class GetAllUsers
    {
        private readonly DatabaseContext _dbContext;
        private readonly Services.UsersService _usersService;

        public GetAllUsers()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersService = new Services.UsersService(_dbContext);
        }

        [Fact]
        public void GetExistingUserShouldSucceed()
        {
            const string password = "Leć Adaś, leć!";
            var user = _dbContext.Users.Add(new User
            {
                Role = UserRole.Tech,
                State = UserState.Active,
                Username = "adam_malysz",
                PasswordHash = Toolbox.ComputeHash(password),
            })
                .Entity;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = _usersService.GetUserByUsernameAndPassword(user.Username, password);

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetNotExistingUserShouldReturnEntityNotFound()
        {
            var result = _usersService.GetUserByUsernameAndPassword("user", "pass");

            result.Status.Should().Be(Status.EntityNotFound);
            result.Object.Should().BeNull();
        }

        [Fact]
        public void GetAllUsersNonShouldExist()
        {
            var result = _usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().BeEmpty();
        }

        [Fact]
        public void GetAllUsersOnlyOneUserExists()
        {
            var user = _dbContext.Users.Add(new User
            {
                Role = UserRole.User,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = _usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().HaveCount(1);
        }

        [Fact]
        public void GetAllUsersOnlyMultipleUsersExists()
        {
            var user = _dbContext.Users.Add(new User
            {
                Role = UserRole.User,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);

            user = _dbContext.Users.Add(new User
            {
                Role = UserRole.User,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);

            user = _dbContext.Users.Add(new User
            {
                Role = UserRole.User,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = _usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().HaveCount(3);
        }

        [Fact]
        public void GetAllDontIncludeAdminAndTech()
        {
            var user = _dbContext.Users.Add(new User
            {
                Role = UserRole.User,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);

            user = _dbContext.Users.Add(new User
            {
                Role = UserRole.Tech,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);

            user = _dbContext.Users.Add(new User
            {
                Role = UserRole.Admin,
                State = UserState.Active,
                Username = "wojtek",
            })
                .Entity;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = _usersService.GetAllUsers();

            result.Status.Should().Be(Status.Success);
            result.Object.Should().HaveCount(1);
        }
    }
}
