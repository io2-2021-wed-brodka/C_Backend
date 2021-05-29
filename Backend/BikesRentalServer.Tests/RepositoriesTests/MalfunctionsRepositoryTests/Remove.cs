using BikesRentalServer.DataAccess;
using BikesRentalServer.DataAccess.Repositories;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Tests.Mock;

namespace BikesRentalServer.Tests.RepositoriesTests.MalfunctionsRepositoryTests
{
    public class Remove
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMalfunctionsRepository _malfunctionsRepository;

        public Remove()
        {
            _dbContext = MockedDbFactory.GetContext();
            _malfunctionsRepository = new MalfunctionsRepository(_dbContext);
        }
    }
}