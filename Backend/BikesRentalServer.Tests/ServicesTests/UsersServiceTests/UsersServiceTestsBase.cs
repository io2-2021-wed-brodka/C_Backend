using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.UsersServiceTests
{
    public class UsersServiceTestsBase
    {
        protected Mock<IUsersRepository> UsersRepository { get; } = new Mock<IUsersRepository>();
        protected Mock<IReservationsRepository> ReservationsRepository { get; } = new Mock<IReservationsRepository>();

        protected IUsersService GetUsersService()
        {
            return new Services.UsersService(UsersRepository.Object, ReservationsRepository.Object);
        }
    }
}
