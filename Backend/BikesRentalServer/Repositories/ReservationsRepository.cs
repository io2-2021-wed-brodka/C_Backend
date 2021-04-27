﻿using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Repositories
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
            return _dbContext.Reservations;
        }

        public Reservation Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;
            return _dbContext.Reservations.SingleOrDefault(r => r.Id == iid);
        }

        public Reservation Add(Reservation entity)
        {
            var reservation = _dbContext.Reservations.Add(entity).Entity;
            _dbContext.SaveChanges();

            return reservation;
        }

        public Reservation Remove(string id)
        {
            var reservation = Get(id);
            if (reservation is null)
                return null;

            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();

            return reservation;
        }

        public Reservation Remove(Reservation entity)
        {
            if (!_dbContext.Reservations.Any(r => r.Id == entity.Id))
                return null;

            var reservation = _dbContext.Reservations.Remove(entity).Entity;
            _dbContext.SaveChanges();

            return reservation;
        }

        public Reservation GetActiveReservation(string bikeId)
        {
            if (!int.TryParse(bikeId, out var iid))
                return null;
            return _dbContext.Reservations.SingleOrDefault(r => r.Bike.Id == iid && r.ExpirationDate > DateTime.Now);
        }
    }
}