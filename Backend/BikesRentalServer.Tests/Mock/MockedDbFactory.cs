using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BikesRentalServer.Tests.Mock
{
    public static class MockedDbFactory
    {
        public static CityBikesDbContext GetContext()
        {
            var ctx = GetContext(GetOptions());
            FillDatabase(ctx);
            return ctx;
        }

        private static CityBikesDbContext GetContext(DbContextOptions<CityBikesDbContext> options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CityBikesDbContext>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

            return null;
        }

        private static DbContextOptions<CityBikesDbContext> GetOptions()
        {
            var options = new DbContextOptionsBuilder<CityBikesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        private static void FillDatabase(CityBikesDbContext context)
        {
            context.Bikes.AddRange(new Bike[]
            {
                new Bike //1
                {
                    Id = 1,
                    Status = BikeStatus.Working,
                    Description = "",
                    User = null,
                    Station = null,
                },
                new Bike //2
                {
                    Id = 2,
                    Status = BikeStatus.Working,
                    Description = "",
                    User = null,
                    Station = null,
                },
                new Bike //3
                {
                    Id = 3,
                    Status = BikeStatus.Working,
                    Description = "",
                    User = null,
                    Station = null,
                }
            });

            context.Stations.AddRange(new Station[]
            {
                new Station //1
                {
                    Id = 1,
                    Status = BikeStationStatus.Working,
                    Bikes = new List<Bike>(),
                    Name = "Aleje Jerozolimskie"
                },
                new Station //2
                {
                    Id = 2,
                    Status = BikeStationStatus.Working,
                    Bikes = new List<Bike>(),
                    Name = "Nowowiejska"
                }
            });

            context.Users.AddRange(new User[]
            {
                new User //1
                {
                    Id = 1,
                    Name = "Wojciech",
                    Lastname = "Szulc",
                    State = UserState.Active,
                    Rentals = new List<Rental>(),
                    Reservations = new List<Reservation>()
                },
                new User //2
                {
                    Id = 2,
                    Name = "Weronika",
                    Lastname = "Bomba",
                    State = UserState.Active,
                    Rentals = new List<Rental>(),
                    Reservations = new List<Reservation>()
                }
            });

            context.SaveChanges();
        }
    }
}
