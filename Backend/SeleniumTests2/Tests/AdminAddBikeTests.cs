using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    // Add bike, remove bike, block bike, unblock bike
    public class AdminAddBikeTests : TestsBase
    {
        public AdminAddBikeTests(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public void AdminAddNewBikeShouldSucceed()
        {
            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();

            var stationName = GetUniqueString();
            adminStationsPage.AddStation(stationName);
            adminStationsPage.ClickOnStation(stationName);
            var oldBikesCount = adminStationsPage.GetUnblockedBikesCount();

            adminStationsPage.AddBikeToOpenedStation();
            var newBikesCount = adminStationsPage.GetUnblockedBikesCount();

            newBikesCount.Should().Be(oldBikesCount + 1);
        }
    }
}
