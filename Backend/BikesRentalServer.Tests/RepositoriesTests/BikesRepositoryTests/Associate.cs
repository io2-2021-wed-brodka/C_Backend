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

            _bikesRepository.Associate(bike.Id.ToString(), user);
            
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

            _bikesRepository.Associate(bike.Id.ToString(), station);
            
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

            var result = _bikesRepository.Associate(bike.Id.ToString(), user);
            
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

            var result = _bikesRepository.Associate(bike.Id.ToString(), station);
            
            result.Should().BeEquivalentTo(bike);
        }

        [Fact]
        public void AssociateNotExistingBikeWithUserShouldReturnNull()
        {
            const string id = "id";

            var result = _bikesRepository.Associate(id, new User());

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateNotExistingBikeWithStationShouldReturnNull()
        {
            const string id = "id";

            var result = _bikesRepository.Associate(id, new Station());

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateWithNotExistingUserShouldReturnNull()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Associate(bike.Id.ToString(), new User
            {
                Username = "null",
            });

            result.Should().BeNull();
        }

        [Fact]
        public void AssociateWithNotExistingStationShouldReturnNull()
        {
            var bike = _dbContext.Bikes.Add(new Bike
                {
                    Status = BikeStatus.Available,
                })
                .Entity;
            _dbContext.SaveChanges();

            var result = _bikesRepository.Associate(bike.Id.ToString(), new Station
            {
                Name = "name",
            });

            result.Should().BeNull();
        }
    }
}
