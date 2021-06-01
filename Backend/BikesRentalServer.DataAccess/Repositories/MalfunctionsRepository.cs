using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
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
            throw new NotImplementedException();
        }

        public Malfunction Add(Malfunction entity)
        {
            var malfunctions = _dbContext.Malfunctions.Add(entity).Entity;
            _dbContext.SaveChanges();
            return malfunctions;
        }

        public Malfunction Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
