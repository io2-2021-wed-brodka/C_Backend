using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class RentalsTests : TestsBase
    {
        public RentalsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task RentBikeShouldSucceed()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.RentBike(bike.Id);

            stationsPage.ContainsSnackbar().Should().BeTrue();
            var rentalsPage = stationsPage.GoToRentals();
            rentalsPage.HasBike(bike.Id).Should().BeTrue();
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

            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            var rentalsPage = stationsPage.GoToRentals();
            rentalsPage.ReturnBike(bike.Id, station.Id);

            rentalsPage.ContainsSnackbar().Should().BeTrue();
            rentalsPage.HasBike(bike.Id).Should().BeFalse();
        }
    }
}
