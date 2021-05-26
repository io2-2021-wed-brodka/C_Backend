using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.Models;
using BikesRentalServer.Tests.Mock;
using FluentAssertions;
using Xunit;

namespace BikesRentalServer.Tests.RepositoriesTests.BikesRepositoryTests
{
    public class Associate
    {
        private readonly DatabaseContext _dbContext;
        private readonly BikesRepository _bikesRepository;
        
        public Associate()
        {
            _dbContext = MockedDbFactory.GetContext();
            _bikesRepository = new BikesRepository(_dbContext);
        }

        [Fact]
        public void AssociateWithUserShouldAssociateWithUser()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "PW",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                    Station = station,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesRepository.AssociateWithUser(bike.Id, user.Id);
            
            bike.User.Should().BeEquivalentTo(user);
            bike.Station.Should().BeNull();
        }

        [Fact]
        public void AssociateWithStationShouldAssociateWithStation()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "PW",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Rented,
                    User = user,
                })
                .Entity;
            _dbContext.SaveChanges();

            _bikesRepository.AssociateWithStation(bike.Id, station.Id);
            
            bike.Station.Should().BeEquivalentTo(station);
            bike.User.Should().BeNull();
        }

        [Fact]
        public void AssociateWithUserShouldReturnChangedBike()
        {
            var user = _dbContext.Users.Add(new User
                {
                    Username = "user",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.AssociateWithUser(bike.Id, user.Id);
            
            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void AssociateWithStationShouldReturnChangedBike()
        {
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "PW",
                })
                .Entity;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.AssociateWithStation(bike.Id, station.Id);
            
            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void AssociateNotExistingBikeWithUserShouldReturnNull()
        {
            const int id = 6;
            var user = _dbContext.Users.Add(new User()).Entity;

            var result = _bikesRepository.AssociateWithUser(id, user.Id);

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateNotExistingBikeWithStationShouldReturnNull()
        {
            const int id = 6;
            var station = _dbContext.Stations.Add(new Station
                {
                    Name = "test",
                })
                .Entity;

            var result = _bikesRepository.AssociateWithStation(id, station.Id);

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateWithNotExistingUserShouldReturnNull()
        {
            const int userId = 7;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.AssociateWithUser(bike.Id, userId);

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateWithNotExistingStationShouldReturnNull()
        {
            const int stationId = 7;
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.AssociateWithStation(bike.Id, stationId);

            result.Should().BeNull();
        }
    }
}
