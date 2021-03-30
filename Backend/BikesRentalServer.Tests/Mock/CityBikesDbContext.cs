using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesRentalServer.Tests.Mock
{
    public class CityBikesDbContext : DbContext
    {
        public CityBikesDbContext(DbContextOptions options) : base(options) { }
        public FakeDbSet<User> Users { get; set; }
        public FakeDbSet<Bike> Bikes { get; set; }
        public FakeDbSet<Station> Stations { get; set; }
        public FakeDbSet<Malfunction> Malfunctions { get; set; }
        public FakeDbSet<Rental> Rentals { get; set; }
        public FakeDbSet<Reservation> Reservations { get; set; }
    }
}
