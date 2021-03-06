using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.BikesServiceTests
{
    public class BikesServiceTestsBase
    {
        protected Mock<IBikesRepository> BikesRepository { get; } = new Mock<IBikesRepository>();
        protected Mock<IUsersRepository> UsersRepository { get; } = new Mock<IUsersRepository>();
        protected Mock<IStationsRepository> StationsRepository { get; } = new Mock<IStationsRepository>();
        protected Mock<IReservationsRepository> ReservationsRepository { get; } = new Mock<IReservationsRepository>();

        protected IBikesService GetBikesService(string username = "maklovitz", UserRole role = UserRole.Admin)
        {
            var userContext = new UserContext();
            userContext.SetOnce(username, role);

            return new Services.BikesService(BikesRepository.Object, StationsRepository.Object, UsersRepository.Object, ReservationsRepository.Object, userContext);
        }
    }
}
