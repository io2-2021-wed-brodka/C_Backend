using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class GetBlockedUsers
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;

        public GetBlockedUsers()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void GetBlockedUsersShouldReturnOnlyUsers()
        {
            var blockedUsers = new[]
            {
                new User
                {
                    Username = "adam",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "marcin",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "krzysztof",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
            };
            _dbContext.Users.AddRange(blockedUsers);
            _dbContext.Users.AddRange(new[]
            {
                new User
                {
                    Username = "piotr",
                    Status = UserStatus.Blocked,
                    Role = UserRole.Admin,
                },
                new User
                {
                    Username = "tomek",
                    Status = UserStatus.Blocked,
                    Role = UserRole.Tech,
                },
            });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetBlockedUsers();

            result.Should().BeEquivalentTo(blockedUsers);
        }

        [Fact]
        public void GetBlockedUsersShouldReturnOnlyBlockedUsers()
        {
            var blockedUsers = new[]
            {
                new User
                {
                    Username = "adam",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "marcin",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "krzysztof",
                    Status = UserStatus.Blocked,
                    Role = UserRole.User,
                },
            };
            _dbContext.Users.AddRange(blockedUsers);
            _dbContext.Users.AddRange(new[]
            {
                new User
                {
                    Username = "piotr",
                    Status = UserStatus.Active,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "tomek",
                    Status = UserStatus.Active,
                    Role = UserRole.User,
                },
            });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetBlockedUsers();

            result.Should().BeEquivalentTo(blockedUsers);
        }

        [Fact]
        public void GetAllShouldReturnEmptyIEnumerableWhenNoUsers()
        {
            var result = _usersRepository.GetBlockedUsers();

            result.Should().BeEmpty();
        }
    }
}
