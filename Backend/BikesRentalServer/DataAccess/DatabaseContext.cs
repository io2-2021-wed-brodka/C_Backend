using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BikesRentalServer.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Malfunction> Malfunctions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
