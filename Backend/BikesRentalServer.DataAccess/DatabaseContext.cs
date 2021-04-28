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
        }
    }
}
