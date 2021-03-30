using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesRentalServer.Services
{
    public class UnitOfWork
    {
        DbContext dbContext;

        public UnitOfWork(DbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
