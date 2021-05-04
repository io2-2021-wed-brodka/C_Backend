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
        public void RemoveUsingIdShouldRemoveUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            _usersRepository.Remove(user.Id.ToString());

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(0);
            _dbContext.Users.SingleOrDefault(x => x.Id == user.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingEntityShouldRemoveUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            _usersRepository.Remove(user);

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(0);
            _dbContext.Users.SingleOrDefault(x => x.Id == user.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveUsingIdShouldReturnRemovedUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.Remove(user.Id.ToString());

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RemoveUsingEntityShouldReturnRemovedUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.Remove(user);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void RemoveUsingInvalidIdShouldRemoveNothingAndReturnNull()
        {
            const string id = "invalid id";
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

        [Fact]
        public void RemoveUsingIdOfNotExistingUserShouldRemoveNothingAndReturnNull()
        {
            const string id = "5";
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

        [Fact]
        public void RemoveNotExistingUserShouldRemoveNothingAndReturnNull()
        {
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
            var result = _usersRepository.Remove(new User
            {
                Username = "piotr",
                Id = 27,
            });

            _dbContext.Users.Count(x => x.Role == UserRole.User).Should().Be(initialUserCount);
            result.Should().BeNull();
        }
    }
}
