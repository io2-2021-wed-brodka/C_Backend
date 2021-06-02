using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminBikeOperationsTests : TestsBase
    {
        public AdminBikeOperationsTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task AdminShouldSeeAllBikes()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bikesTasks = Enumerable.Range(0, 4).Select(_ => Api.AddBike(station.Id, adminToken)).ToArray();
            Task.WaitAll(bikesTasks);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminBikesPage = adminStationsPage.GoToBikes();

            foreach(var bikeTask in bikesTasks)
            {
                adminBikesPage.HasBike(bikeTask.Result.Id).Should().BeTrue();
            }
        }

        [Fact]
        public async Task AdminAddNewBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            adminStationsPage.ClickOnStation(stationName);
            var oldBikesCount = adminStationsPage.GetBikesCount();
            adminStationsPage.AddBikeToOpenedStation();
            var newBikesCount = adminStationsPage.GetBikesCount();

            oldBikesCount.Should().Be(0);
            newBikesCount.Should().Be(1);
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(1);
        }

        [Fact]
        public async Task AdminBlockUnblockedBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminBikesPage = adminStationsPage.GoToBikes();
            var bikeExistsBefore = adminBikesPage.HasBike(bike.Id);
            var bikeBlockedBefore = adminBikesPage.IsBikeBlocked(bike.Id);
            adminBikesPage.BlockBike(bike.Id);
            var bikeExistsAfter = adminBikesPage.HasBike(bike.Id);
            var bikeBlockedAfter = adminBikesPage.IsBikeBlocked(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeBlockedBefore.Should().BeFalse();
            bikeExistsAfter.Should().BeTrue();
            bikeBlockedAfter.Should().BeTrue();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(0);
        }

        [Fact]
        public async Task AdminUnblockBlockedBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            await Api.BlockBike(bike.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminBikesPage = adminStationsPage.GoToBikes();
            var bikeExistsBefore = adminBikesPage.HasBike(bike.Id);
            var bikeBlockedBefore = adminBikesPage.IsBikeBlocked(bike.Id);
            adminBikesPage.UnblockBike(bike.Id);
            var bikeExistsAfter = adminBikesPage.HasBike(bike.Id);
            var bikeBlockedAfter = adminBikesPage.IsBikeBlocked(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeBlockedBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeTrue();
            bikeBlockedAfter.Should().BeFalse();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(1);
        }

        [Fact]
        public async Task AdminRemoveBlockedBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            await Api.BlockBike(bike.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminBikesPage = adminStationsPage.GoToBikes();
            var bikeExistsBefore = adminBikesPage.HasBike(bike.Id);
            adminBikesPage.RemoveBike(bike.Id);
            var bikeExistsAfter = adminBikesPage.HasBike(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeFalse();
            Driver.SwitchToUserTab();
            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(0);
        }

        [Fact]
        public async Task AdminRemoveUnblockedBikeShouldFail()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var adminBikesPage = adminStationsPage.GoToBikes();
            Driver.Sleep();
            var bikeExistsBefore = adminBikesPage.HasBike(bike.Id);
            adminBikesPage.RemoveBike(bike.Id);
            var bikeExistsAfter = adminBikesPage.HasBike(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeTrue();
            adminBikesPage.ContainsSnackbar().Should().BeTrue();
        }
    }
}
