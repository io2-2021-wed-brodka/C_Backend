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
    }
}
