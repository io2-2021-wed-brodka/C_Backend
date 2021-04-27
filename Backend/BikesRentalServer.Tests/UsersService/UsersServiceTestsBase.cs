using BikesRentalServer.Services.Abstract;
using BikesRentalServer.Services;
using BikesRentalServer.Repositories.Abstract;
using Moq;

namespace BikesRentalServer.Tests.UsersServiceTests
{
    public class UsersServiceTestsBase
    {
        protected readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        protected readonly Mock<IReservationsRepository> _reservationsRepository = new Mock<IReservationsRepository>();

        protected UsersServiceTestsBase()
        {
        }

        protected IUsersService GetUsersService()
        {
            return new UsersService(
                _usersRepository.Object,
                _reservationsRepository.Object
                );
        }
    }
}
