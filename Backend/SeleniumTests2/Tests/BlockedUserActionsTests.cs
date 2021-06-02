using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class BlockedUserActionsTests : TestsBase
    {
        public BlockedUserActionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task RentBikeShouldFail()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            var login = GetUniqueString();
            var password = "1234";
            var usrToken = (await Api.SignUp(login, password)).Token;
            var usrId = await Api.GetUserId(login, adminToken);
            _ = await Api.BlockUser(usrId, adminToken);

            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            stationsPage.OpenBikesList(station.Name);
            stationsPage.RentBike(bike.Id);

            stationsPage.ContainsSnackbar().Should().BeTrue();
            var rentalsPage = stationsPage.GoToRentals();
            Driver.Sleep();
            rentalsPage.HasBike(bike.Id).Should().BeFalse();
        }

        [Fact]
        public async Task ReturnBikeShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "123";
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            var signUpResponse = await Api.SignUp(login, password);
            bike = await Api.RentBike(bike.Id, signUpResponse.Token);
            var usrId = await Api.GetUserId(login, adminToken);
            _ = await Api.BlockUser(usrId, adminToken);

            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            var rentalsPage = stationsPage.GoToRentals();
            rentalsPage.ReturnBike(bike.Id, station.Id);

            rentalsPage.ContainsSnackbar().Should().BeTrue();
            rentalsPage.HasBike(bike.Id).Should().BeFalse();
            stationsPage = rentalsPage.GoToStations();
            Driver.Sleep();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.HasBike(bike.Id).Should().BeTrue();
        }

        [Fact]
        public async Task ReserveBikeShouldFail()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            var login = GetUniqueString();
            var password = "1234";
            var usrToken = (await Api.SignUp(login, password)).Token;
            var usrId = await Api.GetUserId(login, adminToken);
            _ = await Api.BlockUser(usrId, adminToken);

            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            Driver.Sleep();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.ReserveBike(bike.Id);

            stationsPage.ContainsSnackbar().Should().BeTrue();
            var reservationsPage = stationsPage.GoToReservations();
            Driver.Sleep();
            reservationsPage.HasBike(bike.Id).Should().BeFalse();
        }
    }
}
