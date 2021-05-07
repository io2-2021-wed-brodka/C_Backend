using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class GetByUsername
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public GetByUsername()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void GetByUsernameShouldReturnUser()
        {
            const string username = "hello_there";
            var user = _dbContext.Users.Add(new User
                {
                    Username = username,
                    Role = UserRole.User,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.GetByUsername(username);
            
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetByUsernameUsingInvalidUsernameShouldReturnNull()
        {
            const string validUsername = "hello_there";
            const string invalidUsername = "hello-there";
            _dbContext.Users.Add(new User
            {
                Username = validUsername,
                Role = UserRole.User,
            });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetByUsername(invalidUsername);
            
            result.Should().BeNull();
        }
    }
}
