using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.StationsServiceTests
{
    public class StationsServiceTestsBase
    {
        protected Mock<IStationsRepository> StationsRepository { get; } = new Mock<IStationsRepository>();
        
        protected IStationsService GetStationsService(string userName = "maklowitz", UserRole role = UserRole.Admin)
        {
            var userContext = new UserContext();
            userContext.SetOnce(userName, role);

            return new Services.StationsService(StationsRepository.Object, userContext);
        }
    }
}
