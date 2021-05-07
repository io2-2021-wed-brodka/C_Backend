using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class Add
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public Add()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void AddShouldAddUserToDatabase()
        {
            _usersRepository.Add(new User());

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(1);
        }

        [Fact]
        public void AddShouldReturnAddedUser()
        {
            const string username = "worst user ever";
            const UserStatus status = UserStatus.Banned;

            var result = _usersRepository.Add(new User
            {
                Role = UserRole.User,
                Username = username,
                Status = status,
            });

            result.Should().NotBeNull();
            result.Id.Should().BePositive();
            result.Username.Should().Be(username);
            result.Status.Should().Be(status);
            result.Role.Should().Be(UserRole.User);
        }
    }
}
