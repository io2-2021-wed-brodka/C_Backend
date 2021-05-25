using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminBikeOperationsTests : TestsBase
    {
        public AdminBikeOperationsTests(ITestOutputHelper output) : base(output)
        {
            
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
            adminStationsPage.ClickOnStation(stationName);
            bool bikeExists = adminStationsPage.HasBike(bike.Id);
            bool bikeBlockedBefore = adminStationsPage.IsBikeBlocked(bike.Id);
            adminStationsPage.BlockBike(bike.Id);
            bool bikeBlockedAfter = adminStationsPage.IsBikeBlocked(bike.Id);

            bikeExists.Should().BeTrue();
            bikeBlockedBefore.Should().BeFalse();
            bikeBlockedAfter.Should().BeTrue();
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
            adminStationsPage.ClickOnStation(stationName);
            bool bikeExistsBefore = adminStationsPage.HasBike(bike.Id);
            bool bikeBlockedBefore = adminStationsPage.IsBikeBlocked(bike.Id);
            adminStationsPage.UnblockBike(bike.Id);
            bool bikeBlockedAfter = adminStationsPage.IsBikeBlocked(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeBlockedBefore.Should().BeTrue();
            bikeBlockedAfter.Should().BeFalse();
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
            adminStationsPage.ClickOnStation(stationName);
            bool bikeExistsBefore = adminStationsPage.HasBike(bike.Id);
            adminStationsPage.ClickRemoveBike(bike.Id);
            bool bikeExistsAfter = adminStationsPage.HasBike(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeFalse();
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
            adminStationsPage.ClickOnStation(stationName);
            bool bikeExistsBefore = adminStationsPage.HasBike(bike.Id);
            adminStationsPage.ClickRemoveBike(bike.Id);
            adminStationsPage.ClickOnStation(stationName);
            bool bikeExistsAfter = adminStationsPage.HasBike(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeTrue();
        }
    }
}
