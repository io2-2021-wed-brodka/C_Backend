using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class GetByUsernameAndPassword
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public GetByUsernameAndPassword()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void GetByUsernameAndPasswordUsingValidCredentialsShouldReturnUser()
        {
            const string username = "hello_there";
            const string password = "G3neraLKen0bi!";
            var user = _dbContext.Users.Add(new User
                {
                    Username = username,
                    PasswordHash = Toolbox.ComputeHash(password),
                    Role = UserRole.User,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.GetByUsernameAndPassword(username, password);
            
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetByUsernameAndPasswordUsingInvalidPasswordShouldReturnNull()
        {
            const string username = "hello_there";
            const string validPassword = "G3neraLKen0bi!";
            const string invalidPassword = "oh,hi!";
            _dbContext.Users.Add(new User
                {
                    Username = username,
                    PasswordHash = Toolbox.ComputeHash(validPassword),
                    Role = UserRole.User,
                });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetByUsernameAndPassword(username, invalidPassword);
            
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUsernameAndPasswordUsingInvalidUsernameShouldReturnNull()
        {
            const string validUsername = "hello_there";
            const string invalidUsername = "hello-there";
            const string password = "G3neraLKen0bi!";
            _dbContext.Users.Add(new User
                {
                    Username = validUsername,
                    PasswordHash = Toolbox.ComputeHash(password),
                    Role = UserRole.User,
                });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetByUsernameAndPassword(invalidUsername, password);
            
            result.Should().BeNull();
        }
    }
}
