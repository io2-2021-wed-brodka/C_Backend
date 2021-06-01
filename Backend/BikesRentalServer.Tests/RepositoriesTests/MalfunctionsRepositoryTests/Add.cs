using BikesRentalServer.DataAccess;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
            string bikeId = "1";
            string reportingUserId = "1";
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
