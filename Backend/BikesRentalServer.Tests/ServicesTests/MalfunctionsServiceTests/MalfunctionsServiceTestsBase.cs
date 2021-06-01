using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikesRentalServer.DataAccess.Repositories.Abstract;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.Services.Abstract;
using Moq;

namespace BikesRentalServer.Tests.ServicesTests.MalfunctionsServiceTests
{
    public class MalfunctionsServiceTestsBase
    {
        protected Mock<IMalfunctionsRepository> MalfunctionRepository { get; } = new Mock<IMalfunctionsRepository>();
        protected Mock<IBikesRepository> BikesRepository { get; } = new Mock<IBikesRepository>();
        protected Mock<IUsersRepository> UsersRepository { get; } = new Mock<IUsersRepository>();
        protected IMalfunctionsService getMalfunctionsService(string username = "maklovitz", UserRole role = UserRole.User)
        {
            var userContext = new UserContext();
            userContext.SetOnce(username, role);
            return new Services.MalfunctionsService(userContext, MalfunctionRepository.Object, BikesRepository.Object, UsersRepository.Object );
        }
    }
}
