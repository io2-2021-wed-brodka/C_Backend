using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.DataAccess.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly DatabaseContext _dbContext;
        
        public ReservationsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Reservation> GetAll()
        {
            return _dbContext.Reservations
                .Include(r => r.Bike)
                .Include(r => r.User);
        }

        public Reservation Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            
            return _dbContext.Reservations
                .Include(r => r.Bike)
                .Include(r => r.User)
                .SingleOrDefault(r => r.Id == iid);
        }

        public Reservation Add(Reservation entity)
        {
            var reservation = _dbContext.Reservations.Add(entity).Entity;
            _dbContext.SaveChanges();

            return reservation;
        }

        public Reservation Remove(int id)
        {
            var reservation = Get(id);
            if (reservation is null)
                return null;

            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();

            return reservation;
        }

        public Reservation GetActiveReservation(int bikeId)
        {
            return _dbContext.Reservations
                .Include(r => r.Bike)
                .Include(r => r.User)
                .SingleOrDefault(r => r.Bike.Id == bikeId && r.ExpirationDate > DateTime.Now);
        }

        public IEnumerable<Reservation> GetActiveReservations(int userId)
        {
            return _dbContext.Reservations
                .Include(r => r.Bike)
                .Include(r => r.User)
                .Where(r => r.User.Id == userId && r.ExpirationDate > DateTime.Now);
        }

        private Reservation Get(int id) => Get(id.ToString());
    }
}
