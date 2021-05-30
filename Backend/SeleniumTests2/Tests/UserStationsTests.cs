using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesRentalServer.WebApi.Dtos.Responses;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class UserStationsTests : TestsBase
    {
        public UserStationsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ClickingOnStationShouldDisplayBikesList()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);

            var bikes = new List<GetBikeResponse>
            {
                await Api.AddBike(station.Id, adminToken),
                await Api.AddBike(station.Id, adminToken),
                await Api.AddBike(station.Id, adminToken),
                await Api.AddBike(station.Id, adminToken)
            };

            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(station.Name);
            bikes.All(bike => stationsPage.HasBike(bike.Id)).Should().BeTrue();
        }                                                        
    }                                                            
}
