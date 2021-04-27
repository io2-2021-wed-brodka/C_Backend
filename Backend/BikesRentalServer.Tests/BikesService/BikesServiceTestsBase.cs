using BikesRentalServer.Authorization;
using BikesRentalServer.Models;
using BikesRentalServer.Repositories.Abstract;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.BikesService
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
