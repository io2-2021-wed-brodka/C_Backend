using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Services;
using BikesRentalServer.Repositories.Abstract;
using Moq;
using BikesRentalServer.Authorization;
using BikesRentalServer.Models;
using BikesRentalServer.DataAccess;

namespace BikesRentalServer.Tests.BikesServiceTests
{
    public class BikesServiceTestsBase
    {
        protected readonly Mock<IBikesRepository> _bikesRepository = new Mock<IBikesRepository>();
        protected readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        protected readonly Mock<IStationsRepository> _stationsRepository = new Mock<IStationsRepository>();
        protected readonly Mock<IReservationsRepository> _reservationsRepository = new Mock<IReservationsRepository>();

        protected BikesServiceTestsBase()
        {
        }

        protected IBikesService GetBikesService()
        {
            return GetBikesService("maklowitz");
        }

        protected IBikesService GetBikesService(string userName, UserRole role = UserRole.Admin)
        {
            var userContext = new UserContext();
            userContext.SetOnce(userName, role);

            return new BikesService(
                _bikesRepository.Object,
                _stationsRepository.Object,
                _usersRepository.Object,
                _reservationsRepository.Object,
                userContext
                );
        }
    }
}
