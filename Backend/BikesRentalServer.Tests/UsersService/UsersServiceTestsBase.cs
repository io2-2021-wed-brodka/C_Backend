using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Services;
using BikesRentalServer.Repositories.Abstract;
using Moq;
using BikesRentalServer.Authorization;
using BikesRentalServer.Models;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class UsersServiceTestsBase
    {
        protected readonly Mock<IBikesRepository> _bikesRepository = new Mock<IBikesRepository>();
        protected readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        protected readonly Mock<IStationsRepository> _stationsRepository = new Mock<IStationsRepository>();
        protected readonly Mock<IReservationsRepository> _reservationsRepository = new Mock<IReservationsRepository>();

        protected UsersServiceTestsBase()
        {
        }

        protected IBikesService GetUsersService()
        {
            return GetUsersService("maklowitz");
        }

        protected IBikesService GetUsersService(string userName, UserRole role = UserRole.Admin)
        {
            var userContext = new UserContext();
            userContext.SetOnce(userName, role);

            return new UsersService(
                _bikesRepository.Object,
                _stationsRepository.Object,
                _usersRepository.Object,
                _reservationsRepository.Object,
                userContext
                );
        }
    }
}
