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
            stationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public async Task NewStationLimitShouldWorkForAdmin()
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
        public async Task StationShouldNotBeRemovableWhenItHasBikes()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            await Api.AddBike(station.Id, adminToken);
            await Api.BlockStation(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

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

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickRemoveStation(stationName);

            adminStationsPage.ContainsSnackbar().Should().BeTrue();
            adminStationsPage.HasStation(stationName).Should().BeTrue();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
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

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickRemoveStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeFalse();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
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

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickBlockStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeTrue();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
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

            adminStationsPage.ClickOnStation(stationName);
            adminStationsPage.ClickUnblockStation(stationName);

            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.HasStation(stationName).Should().BeTrue();
        }
    }
}
