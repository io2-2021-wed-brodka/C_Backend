using System.Collections.Generic;
using System.Linq;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;
using Microsoft.EntityFrameworkCore;

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
            throw new System.NotImplementedException();
        }

        public Malfunction Get(string id)
        {
            if (!int.TryParse(id, out var iid))
                return null;

            return Get(id);
        }

        public Malfunction Add(Malfunction entity)
        {
            throw new System.NotImplementedException();
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