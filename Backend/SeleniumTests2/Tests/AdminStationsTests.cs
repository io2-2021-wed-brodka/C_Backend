using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminStationsTests : TestsBase
    {
        public AdminStationsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void NewStationShouldBeOnList()
        {
            var stationName = GetUniqueString();

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            adminStationsPage.AddStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public async Task NewStationShouldBeVisibleToUser()
        {
            var stationName = GetUniqueString();

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            adminStationsPage.AddStation(stationName);

            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public void NewStationLimitShouldWorkForAdmin()
        {
            var stationName = GetUniqueString();

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            adminStationsPage.AddStation(stationName, 2);
            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.AddBikeToOpenedStation();
            adminStationsPage.ContainsSnackbar().Should().BeFalse();
            adminStationsPage.AddBikeToOpenedStation();
            adminStationsPage.ContainsSnackbar().Should().BeFalse();
            adminStationsPage.AddBikeToOpenedStation();
            adminStationsPage.ContainsSnackbar().Should().BeTrue();
            adminStationsPage.GetBikesCount().Should().Be(2);
        }

        [Fact]
        public async Task NewStationLimitShouldWorkForUser()
        {
            var login = GetUniqueString();
            var password = "234";
            var stationName = GetUniqueString();
            var station2Name = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken, 2);
            var station2 = await Api.AddStation(station2Name, adminToken, 3);
            var bike1 = await Api.AddBike(station2.Id, adminToken);
            var bike2 = await Api.AddBike(station2.Id, adminToken);
            var bike3 = await Api.AddBike(station2.Id, adminToken);
            var user = await Api.SignUp(login, password);
            await Api.RentBike(bike1.Id, user.Token);
            await Api.RentBike(bike2.Id, user.Token);
            await Api.RentBike(bike3.Id, user.Token);
            
            new LoginPage(Driver).LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            var rentalsPage = stationsPage.GoToRentals();
            Driver.Sleep();

            rentalsPage.ReturnBike(bike1.Id, station.Id);
            rentalsPage.HasBike(bike1.Id).Should().BeFalse();
            rentalsPage.ReturnBike(bike2.Id, station.Id);
            rentalsPage.HasBike(bike2.Id).Should().BeFalse();
            rentalsPage.ReturnBike(bike3.Id, station.Id);
            rentalsPage.HasBike(bike3.Id).Should().BeTrue();
            rentalsPage.ContainsSnackbar().Should().BeTrue();
        }

        [Fact]
        public async Task StationShouldNotBeRemovableWhenItHasBikes()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            await Api.AddBike(station.Id, adminToken);
            await Api.BlockStation(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            Driver.Sleep();

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickRemoveStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeTrue();
            adminStationsPage.ContainsSnackbar().Should().BeTrue();
        }

        [Fact]
        public async Task StationShouldNotBeRemovableWhenItHasNoBikeAndItIsActive()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            Driver.Sleep();

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickRemoveStation(stationName);

            adminStationsPage.ContainsSnackbar().Should().BeTrue();
            adminStationsPage.HasStation(stationName).Should().BeTrue();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public async Task StationShouldBeRemovableWhenItHasNoBikeAndItIsBlocked()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            await Api.BlockStation(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            Driver.Sleep();

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickRemoveStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeFalse();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeFalse();
        }

        [Fact]
        public async Task StationShouldNotBeVisibleToUserWhenItIsBlocked()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            await Api.BlockStation(station.Id, adminToken);

            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeFalse();
        }

        [Fact]
        public async Task StationShouldNotBeVisibleToUserAfterItIsBlocked()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            Driver.Sleep();

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickBlockStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeTrue();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeFalse();
        }

        [Fact]
        public async Task StationShouldBeVisibleToUserAfterItIsUnblocked()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            await Api.BlockStation(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            Driver.Sleep();

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickUnblockStation(stationName);

            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            Driver.Sleep();
            stationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public async Task BikesCountsShouldBeDisplayed()
        {
            var stationName = GetUniqueString();
            var login = GetUniqueString();
            var password = "23456";
            var userToken = await Api.SignUp(login, password);
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike1 = await Api.AddBike(station.Id, adminToken);
            var bike2 = await Api.AddBike(station.Id, adminToken);
            var bike3 = await Api.AddBike(station.Id, adminToken);
            var bike4 = await Api.AddBike(station.Id, adminToken);

            await Api.ReserveBike(bike2.Id, userToken.Token);
            await Api.RentBike(bike1.Id, userToken.Token);
            await Api.ReportMalfunction(bike1.Id, "It does not work", userToken.Token);
            await Api.ReturnBike(bike1.Id, station.Id, userToken.Token);


            Driver.OpenAdminTab();
            var stationsPage = LoginAsAdmin();
            Driver.Sleep();
            stationsPage.GetActiveBikesCount(stationName).Should().Be(3);
            stationsPage.GetBrokenBikesCount(stationName).Should().Be(1);
            stationsPage.GetReservedBikesCount(stationName).Should().Be(1);
        }
    }
}
