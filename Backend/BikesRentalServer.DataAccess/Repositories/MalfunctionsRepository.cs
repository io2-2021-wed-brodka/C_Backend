using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Malfunction Add(Malfunction entity)
        {
            var malfunctions = _dbContext.Malfunctions.Add(entity).Entity;
            _dbContext.SaveChanges();
            return malfunctions;
        }

        public Malfunction Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Malfunction> GetAll()
        {
            throw new NotImplementedException();
        }

        public Malfunction Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Malfunction Remove(Malfunction entity)
        {
            throw new NotImplementedException();
        }
    }
}
