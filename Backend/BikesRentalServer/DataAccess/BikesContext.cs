using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EFDataAccessLibrary.Models;

namespace EFDataAccessLibrary.DataAccess
{
    public class BikesContext : DbContext
    {
        public BikesContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<BikeStation> BikeStations { get; set; }
        public DbSet<Malfunction> Malfunctions { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
