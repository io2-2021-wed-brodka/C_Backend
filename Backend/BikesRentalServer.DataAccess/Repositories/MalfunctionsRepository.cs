using System.Collections.Generic;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories
{
    public class MalfunctionsRepository : IMalfunctionsRepository
    {
        private readonly DatabaseContext _dbContext;

        public MalfunctionsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Malfunction> GetAll()
        {
            return _dbContext.Malfunctions
                .Include(m => m.Bike)
                .Include(m => m.ReportingUser);
        }

        public Malfunction Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;

            return Get(iid);
        }

        public Malfunction Add(Malfunction entity)
        {
            var malfunctions = _dbContext.Malfunctions.Add(entity).Entity;
            _dbContext.SaveChanges();
            return malfunctions;
        }

        public Malfunction Remove(int id)
        {
            var malfunction = Get(id);
            if (malfunction is null)
                return null;

            _dbContext.Malfunctions.Remove(malfunction);
            _dbContext.SaveChanges();

            return malfunction;
        }

        private Malfunction Get(int id)
        {
            return _dbContext.Malfunctions
                .Include(m => m.Bike)
                .Include(m => m.ReportingUser)
                .SingleOrDefault(m => m.Id == id);
        }
    }
}
