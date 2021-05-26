using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.UsersRepositoryTests
{
    public class SetStatus
    {
        private readonly DatabaseContext _dbContext;
        private readonly UsersRepository _usersRepository;
        
        public SetStatus()
        {
            _dbContext = MockedDbFactory.GetContext();
            _usersRepository = new UsersRepository(_dbContext);
        }

        [Fact]
        public void SetStatusShouldSetStatus()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "jedi",
                    Status = UserStatus.Active,
                })
                .Entity;
            _dbContext.SaveChanges();

            _usersRepository.SetStatus(user.Id, UserStatus.Blocked);

            user.Status.Should().Be(UserStatus.Blocked);
        }

        [Fact]
        public void SetStatusShouldReturnChangedUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "jedi",
                    Status = UserStatus.Active,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _usersRepository.SetStatus(user.Id, UserStatus.Blocked);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void SetStatusOfNotExistingUserShouldReturnNull()
        {
            const int id = 7;
            
            var result = _usersRepository.SetStatus(id, UserStatus.Active);

            result.Should().BeNull();
        }
    }
}
