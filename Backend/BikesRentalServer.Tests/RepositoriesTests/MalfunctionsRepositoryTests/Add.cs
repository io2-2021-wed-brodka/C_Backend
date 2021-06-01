using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.MalfunctionsRepositoryTests
{
    public class Add
    {
        private readonly DatabaseContext _dbContext;
        private readonly MalfunctionsRepository _malfunctionsRepository;

        public Add()
        {
            _dbContext = MockedDbFactory.GetContext();
            _malfunctionsRepository = new MalfunctionsRepository(_dbContext);
        }

        [Fact]
        public void AddShouldAddMalfunctionToDatabase()
        {
            
            _malfunctionsRepository.Add(new Malfunction());
            _dbContext.Malfunctions.Count().Should().Be(1);
        }

        [Fact]
        public void AddShouldReturnAddedMalfunction()
        {
            const string description = "there is no wheel in this bicycle";
            var user = _dbContext.Users.Add(new User()).Entity;
            var bike = _dbContext.Bikes.Add(new Bike()).Entity;
            _dbContext.SaveChanges();

            var result = _malfunctionsRepository.Add(new Malfunction
            {
                Bike = bike,
                ReportingUser = user,
                Description = description,
                State = MalfunctionState.NotFixed
            });

            result.Should().NotBeNull();
            result.Id.Should().BePositive();
            result.Description.Should().Be(description);
            result.Bike.Should().BeEquivalentTo(bike);
            result.ReportingUser.Should().BeEquivalentTo(user);
            result.State.Should().Be(MalfunctionState.NotFixed);
        }
    }
}
