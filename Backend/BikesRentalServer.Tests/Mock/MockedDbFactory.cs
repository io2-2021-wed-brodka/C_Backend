using BikesRentalServer.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;

namespace BikesRentalServer.Tests.Mock
{
    public static class MockedDbFactory
    {
        public static DatabaseContext GetContext()
        {
            var ctx = new DatabaseContext(GetOptions());
            return ctx;
        }

        private static DbContextOptions<DatabaseContext> GetOptions()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return options;
        }
    }
}
