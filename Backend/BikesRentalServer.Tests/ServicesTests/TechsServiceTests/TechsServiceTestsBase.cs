using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.TechsServiceTests
{
    public class TechsServiceTestsBase
    {
        protected Mock<IBikesRepository> BikesRepository { get; } = new Mock<IBikesRepository>();
        protected Mock<IMalfunctionsRepository> MalfunctionsRepository { get; } = new Mock<IMalfunctionsRepository>();
        
        protected IMalfunctionsService GetTechsService()
        {
            return new Services.MalfunctionsService(MalfunctionsRepository.Object);
        }
    }
}
