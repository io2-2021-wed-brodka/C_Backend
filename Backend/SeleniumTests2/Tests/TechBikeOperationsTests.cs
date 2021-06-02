using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class TechBikeOperationsTests : TestsBase
    {
        public TechBikeOperationsTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task TechShouldSeeAllBikes()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bikesTasks = Enumerable.Range(0, 4).Select(_ => Api.AddBike(station.Id, adminToken)).ToArray();
            Task.WaitAll(bikesTasks);

            var stationsPage = await LoginAsSomeTech();
            var techBikesPage = stationsPage.GoToTechBikes();

            foreach(var bikeTask in bikesTasks)
            {
                techBikesPage.HasBike(bikeTask.Result.Id).Should().BeTrue();
            }
        }

        [Fact]
        public async Task TechBlockUnblockedBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            var stationsPage = await LoginAsSomeTech();
            var techBikesPage = stationsPage.GoToTechBikes();
            Driver.Sleep();
            var bikeExistsBefore = techBikesPage.HasBike(bike.Id);
            var bikeBlockedBefore = techBikesPage.IsBikeBlocked(bike.Id);
            techBikesPage.BlockBike(bike.Id);
            var bikeExistsAfter = techBikesPage.HasBike(bike.Id);
            var bikeBlockedAfter = techBikesPage.IsBikeBlocked(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeBlockedBefore.Should().BeFalse();
            bikeExistsAfter.Should().BeTrue();
            bikeBlockedAfter.Should().BeTrue();
            techBikesPage.LogOut();
            stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(0);
        }

        [Fact]
        public async Task TechUnblockBlockedBikeShouldSucceed()
        {
            var stationName = GetUniqueString();
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            await Api.BlockBike(bike.Id, adminToken);

            var stationsPage = await LoginAsSomeTech();
            var techBikesPage = stationsPage.GoToTechBikes();
            Driver.Sleep();
            var bikeExistsBefore = techBikesPage.HasBike(bike.Id);
            var bikeBlockedBefore = techBikesPage.IsBikeBlocked(bike.Id);
            techBikesPage.UnblockBike(bike.Id);
            var bikeExistsAfter = techBikesPage.HasBike(bike.Id);
            var bikeBlockedAfter = techBikesPage.IsBikeBlocked(bike.Id);

            bikeExistsBefore.Should().BeTrue();
            bikeBlockedBefore.Should().BeTrue();
            bikeExistsAfter.Should().BeTrue();
            bikeBlockedAfter.Should().BeFalse();
            techBikesPage.LogOut();
            stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(stationName);
            stationsPage.GetBikesCount().Should().Be(1);
        }
    }
}
