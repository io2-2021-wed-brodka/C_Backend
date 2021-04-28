using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Services;
using BikesRentalServer.Repositories.Abstract;
using Moq;
using BikesRentalServer.Authorization;
using BikesRentalServer.Models;

namespace BikesRentalServer.Tests.StationsServiceTests
{
    public class StationsServiceTestsBase
    {
        protected readonly Mock<IStationsRepository> _stationsRepository = new Mock<IStationsRepository>();

        protected StationsServiceTestsBase()
        {
        }

        protected IStationsService GetStationsService()
        {
            return GetStationsService("maklowitz");
        }

        protected IStationsService GetStationsService(string userName, UserRole role = UserRole.Admin)
        {
            var userContext = new UserContext();
            userContext.SetOnce(userName, role);

            return new StationsService(
                _stationsRepository.Object,
                userContext
                );
        }
    }
}
