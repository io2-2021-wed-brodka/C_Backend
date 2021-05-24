using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SeleniumTests2
{
    public class RentalsTests : TestsBase
    {
        public RentalsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ReturnBike()
        {
            var login = GetUniqueString();
            var password = "123";
            var stationName = GetUniqueString();
            var adminToken = await _api.LogInAsAdmin();
            var station = await _api.AddStation(stationName, adminToken);
            var bike = await _api.AddBike(station.Id, adminToken);
            var signUpResponse = await _api.SignUp(login, password);
            bike = await _api.RentBike(bike.Id, signUpResponse.Token);

            var loginPage = new LoginPage(_driver);
            loginPage.LogIn(login, password);
            var stationsPage = new StationsPage(_driver);
            var rentalsPage = stationsPage.GoToRentals();
            rentalsPage.ReturnBike(bike.Id, station.Id);

            rentalsPage.ContainsSnackbar().Should().BeTrue();
            rentalsPage.HasBike(bike.Id).Should().BeFalse();
        }
    }
}
