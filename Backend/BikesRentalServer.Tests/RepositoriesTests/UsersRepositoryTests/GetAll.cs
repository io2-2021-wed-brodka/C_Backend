using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class GetAll
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public GetAll()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void GetAllShouldReturnAllAndOnlyUsers()
        {
            var users = new []
            {
                new User
                {
                    Username = "adam",
                    Status = UserStatus.Active,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "marcin",
                    Status = UserStatus.Active,
                    Role = UserRole.User,
                },
                new User
                {
                    Username = "krzysztof",
                    Status = UserStatus.Active,
                    Role = UserRole.User,
                },
            };
            _dbContext.Users.AddRange(users);
            _dbContext.Users.AddRange(new []
            {
                new User
                {
                    Username = "piotr",
                    Status = UserStatus.Active,
                    Role = UserRole.Admin,
                },
                new User
                {
                    Username = "tomek",
                    Status = UserStatus.Active,
                    Role = UserRole.Tech,
                },
            });
            _dbContext.SaveChanges();

            var result = _usersRepository.GetAll();
            
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetAllShouldReturnEmptyIEnumerableWhenNoBikes()
        {
            var result = _usersRepository.GetAll();
            
            result.Should().BeEmpty();
        }
    }
}
