using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class Get
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public Get()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void GetShouldReturnRequestedUserForValidId()
        {
            const int id = 2;
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                    Role = UserRole.User,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.Get(id.ToString());
            
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetShouldReturnNullIfNoUsersWithProvidedId()
        {
            const string id = "100";

            var result = _usersRepository.Get(id);

            result.Should().BeNull();
        }

        [Fact]
        public void GetShouldReturnNullForInvalidId()
        {
            const string id = "invalid id";

            var result = _usersRepository.Get(id);

            result.Should().BeNull();
        }
    }
}
