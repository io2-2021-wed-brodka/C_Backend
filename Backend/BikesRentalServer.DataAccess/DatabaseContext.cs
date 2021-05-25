using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BikesRentalServer.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            // Some weird stuff happening with sqlite. In case of changes use trial-and-error with lines below to actually migrate.
            Database.EnsureCreated();
            // Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Malfunction> Malfunctions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Role = UserRole.Admin,
                    Username = "admin",
                    PasswordHash = Toolbox.ComputeHash("admin"),
                });

            var station1 = new Station
            {
                Id = 1,
                Name = "Wenus",
                Status = StationStatus.Active,
            };

            var station2 = new Station
            {
                Id = 2,
                Name = "Planeta Małp",
                Status = StationStatus.Active,
            };

            var station3 = new Station
            {
                Id = 3,
                Name = "Mars",
                Status = StationStatus.Active,
            };

            modelBuilder.Entity<Station>().HasData(
                station1,
                station2,
                station3
            );

            modelBuilder.Entity<Bike>()
                .HasData(new Bike
                {
                    Id = 1,
                    StationId = 2,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Id = 2,
                    StationId = 3,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Id = 3,
                    StationId = 1,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Id = 4,
                    StationId = 1,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Id = 5,
                    StationId = 2,
                    Status = BikeStatus.Available,
                },
                new Bike
                {
                    Id = 6,
                    StationId = 3,
                    Status = BikeStatus.Available,
                });
        }
    }
}
