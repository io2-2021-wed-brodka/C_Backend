using System;
using System.Linq;
using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.MalfunctionsRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMalfunctionsRepository _malfunctionsRepository;

        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _malfunctionsRepository = new MalfunctionsRepository(_dbContext);
        }

        [Fact]
        public void RemoveExistingMalfunction_ShouldSucceed()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "Stefan",
                })
                .Entity;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Available,
                })
                .Entity;
            var malfunction = _dbContext.Malfunctions.Add(new Malfunction
                {
                    Bike = bike,
                    Description = "flat tire",
                    DetectionDate = new DateTime(2021, 5, 29, 12, 52, 7),
                    ReportingUser = user,
                    State = MalfunctionState.NotFixed,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _malfunctionsRepository.Remove(malfunction.Id);
            
            result.Should().BeEquivalentTo(malfunction);
            _dbContext.Malfunctions.Count().Should().Be(0);
        }

        [Fact]
        public void RemoveExistingMalfunction_ShouldReturnRemovedMalfunction()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "Stefan",
                })
                .Entity;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Available,
                })
                .Entity;
            var malfunction = _dbContext.Malfunctions.Add(new Malfunction
                {
                    Bike = bike,
                    Description = "flat tire",
                    DetectionDate = new DateTime(2021, 5, 29, 12, 52, 7),
                    ReportingUser = user,
                    State = MalfunctionState.NotFixed,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _malfunctionsRepository.Remove(malfunction.Id);
            
            _dbContext.Malfunctions.SingleOrDefault(x => x.Id == malfunction.Id).Should().BeNull();
        }

        [Fact]
        public void RemoveNotExistingMalfunction_ShouldRemoveNothingAndReturnNull()
        {
            const int id = 5;
            var user = _dbContext.Users.Add(new User
                {
                    Username = "Stefan",
                })
                .Entity;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "station",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Station = station,
                    Status = BikeStatus.Available,
                })
                .Entity;
            var malfunction = _dbContext.Malfunctions.Add(new Malfunction
                {
                    Id = 77,
                    Bike = bike,
                    Description = "flat tire",
                    DetectionDate = new DateTime(2021, 5, 29, 12, 52, 7),
                    ReportingUser = user,
                    State = MalfunctionState.NotFixed,
                })
                .Entity;
            _dbContext.SaveChanges();

            var initialMalfunctionCount = _dbContext.Malfunctions.Count();
            var result = _malfunctionsRepository.Remove(id);
            
            _dbContext.Malfunctions.Count().Should().Be(initialMalfunctionCount);
            result.Should().BeNull();
        }
    }
}
