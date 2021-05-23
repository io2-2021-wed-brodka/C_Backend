using System;
using Xunit;
using System.Threading;
using FluentAssertions;
using System.Threading.Tasks;

namespace SeleniumTests2
{
    public class AdminStationsTests : TestsBase
    {
        [Fact]
        public void NewStationShouldBeOnList()
        {
            var stationName = GetUniqueString();

            _driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            adminStationsPage.AddStation(stationName);

            adminStationsPage.HasStation(stationName).Should().BeTrue();
        }

        [Fact]
        public async Task NewStationShouldBeVisibleToUser()
        {
            var stationName = GetUniqueString();

            _driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            adminStationsPage.AddStation(stationName);

            _driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.HasStation(stationName).Should().BeTrue();
        }
    }
}
