using System.Collections.Generic;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Models;

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
            throw new System.NotImplementedException();
        }

        public Malfunction Add(Malfunction entity)
        {
            throw new System.NotImplementedException();
        }

        public Malfunction Remove(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}