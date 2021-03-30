using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BikesRentalServer.Tests.Mock
{
    public static class MockedDbFactory
    {
        public static DatabaseContext GetContext()
        {
            var ctx = new DatabaseContext(GetOptions());
            FillDatabase(ctx);
            return ctx;
        }

        private static DbContextOptions<DatabaseContext> GetOptions()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return options;
        }

        private static void FillDatabase(DatabaseContext context)
        {
            context.Users.Add(new User
            {
                Id = 1,
                Name = "Wojciech",
                Lastname = "Szulc",
                State = UserState.Active,
                Rentals = new List<Rental>(),
                Reservations = new List<Reservation>()
            });
            context.Users.Add(new User
            {
                Id = 2,
                Name = "Weronika",
                Lastname = "Bomba",
                State = UserState.Active,
                Rentals = new List<Rental>(),
                Reservations = new List<Reservation>()
            });
            context.Stations.Add(new Station
            {
                Id = 0,
                Status = BikeStationStatus.Working,
                Bikes = new List<Bike>(),
                Name = "Al. Jerozolimskie"
            });
            context.SaveChanges();

            // Some other data to use
            //context.Bikes.AddRange(new Bike[]
            //{
            //    new Bike //1
            //    {
            //        Id = 1,
            //        Status = BikeStatus.Working,
            //        Description = "",
            //        User = null,
            //        Station = null,
            //    },
            //    new Bike //2
            //    {
            //        Id = 2,
            //        Status = BikeStatus.Working,
            //        Description = "",
            //        User = null,
            //        Station = null,
            //    },
            //    new Bike //3
            //    {
            //        Id = 3,
            //        Status = BikeStatus.Working,
            //        Description = "",
            //        User = null,
            //        Station = null,
            //    }
            //});

            //context.Stations.AddRange(new Station[]
            //{
            //    new Station //1
            //    {
            //        Id = 1,
            //        Status = BikeStationStatus.Working,
            //        Bikes = new List<Bike>(),
            //        Name = "Aleje Jerozolimskie"
            //    },
            //    new Station //2
            //    {
            //        Id = 2,
            //        Status = BikeStationStatus.Working,
            //        Bikes = new List<Bike>(),
            //        Name = "Nowowiejska"
            //    }
            //});

            //context.Users.AddRange(new User[]
            //{
            //    new User //1
            //    {
            //        Id = 1,
            //        Name = "Wojciech",
            //        Lastname = "Szulc",
            //        State = UserState.Active,
            //        Rentals = new List<Rental>(),
            //        Reservations = new List<Reservation>()
            //    },
            //    new User //2
            //    {
            //        Id = 2,
            //        Name = "Weronika",
            //        Lastname = "Bomba",
            //        State = UserState.Active,
            //        Rentals = new List<Rental>(),
            //        Reservations = new List<Reservation>()
            //    }
            //});

            //context.SaveChanges();
        }
    }
}
