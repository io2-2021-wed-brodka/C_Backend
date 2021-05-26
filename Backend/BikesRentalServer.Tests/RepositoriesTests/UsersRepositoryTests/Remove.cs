using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void RemoveShouldRemoveUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            _usersRepository.Remove(user.Id);

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(0);
            _dbContext.Users.SingleOrDefault(x => x.Id == user.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveShouldReturnRemovedUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.Remove(user.Id);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RemoveUsingIdOfNotExistingUserShouldRemoveNothingAndReturnNull()
        {
            const int id = 5;
            _dbContext.Users.AddRange(new []
            {
                new User
                {
                    Username = "user1",
                },
                new User
                {
                    Username = "user2",
                },
                new User
                {
                    Username = "user3",
                },
            });
            _dbContext.SaveChanges();

            var initialUserCount = _dbContext.Users.Count(x => x.Role == UserRole.User);
            var result = _usersRepository.Remove(id);

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(initialUserCount);
            result.Should().BeNull();
        }
    }
}
