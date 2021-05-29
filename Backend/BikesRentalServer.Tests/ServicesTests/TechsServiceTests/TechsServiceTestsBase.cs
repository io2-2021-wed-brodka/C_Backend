using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.TechsServiceTests
{
    public class TechsServiceTestsBase
    {
        protected Mock<IUsersRepository> UsersRepository { get; } = new Mock<IUsersRepository>();
        protected Mock<IBikesRepository> BikesRepository { get; } = new Mock<IBikesRepository>();
        protected Mock<IMalfunctionsRepository> MalfunctionsRepository { get; } = new Mock<IMalfunctionsRepository>();
        
        protected ITechsService GetTechsService(string userName = "maklowitz", UserRole role = UserRole.Tech)
        {
            var userContext = new UserContext();
            userContext.SetOnce(userName, role);
            
            return new Services.TechsService(UsersRepository.Object, BikesRepository.Object, MalfunctionsRepository.Object, userContext);
        }
    }
}