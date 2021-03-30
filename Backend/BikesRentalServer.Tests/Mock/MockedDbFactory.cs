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
            var cnt = context.Users.CountAsync();
            cnt.Wait();

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
                Id = 1,
                Status = BikeStationStatus.Working,
                Bikes = new List<Bike>(),
                Name = "Al. Jerozolimskie"
            });
            context.SaveChanges();
        }
    }
}
