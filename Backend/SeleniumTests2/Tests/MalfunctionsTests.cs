using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class MalfunctionsTests : TestsBase
    {
        public MalfunctionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task UserShouldBeAbleToReportMalfunction()
        {
            var malfunctionMessage = GetUniqueString();
            var stationName = GetUniqueString();
            var login = GetUniqueString();
            var password = "123";
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            var user = await Api.SignUp(login, password);
            await Api.RentBike(bike.Id, user.Token);

            new LoginPage(Driver).LogIn(login, password);
            var stationsPage = new StationsPage(Driver);
            var rentalsPage = stationsPage.GoToRentals();
            rentalsPage.ReportMalfunction(bike.Id, malfunctionMessage);
            rentalsPage.ContainsSnackbar().Should().BeTrue();
            rentalsPage.LogOut();

            var malfunctionsResponse = await Api.GetMalfunctions(adminToken);
            var malfunction = malfunctionsResponse.Malfunctions.Single(malfunction => malfunction.Description == malfunctionMessage);

            stationsPage = await LoginAsSomeTech();
            var malfunctionsPage = stationsPage.GoToTechMalfunctions();
            malfunctionsPage.HasMalfunction(malfunction.Id).Should().BeTrue();
        }

        [Fact]
        public async Task TechShouldBeAbleToRemoveMalfunction()
        {
            var malfunctionMessage = GetUniqueString();
            var stationName = GetUniqueString();
            var login = GetUniqueString();
            var password = "123";
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(stationName, adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);
            var user = await Api.SignUp(login, password);
            await Api.RentBike(bike.Id, user.Token);
            await Api.ReportMalfunction(bike.Id, malfunctionMessage, user.Token);

            var malfunctionsResponse = await Api.GetMalfunctions(adminToken);
            var malfunction = malfunctionsResponse.Malfunctions.Single(malfunction => malfunction.Description == malfunctionMessage);

            var stationsPage = await LoginAsSomeTech();
            var malfunctionsPage = stationsPage.GoToTechMalfunctions();
            malfunctionsPage.RemoveMalfunction(malfunction.Id);
            malfunctionsPage.HasMalfunction(malfunction.Id).Should().BeFalse();
            malfunctionsResponse = await Api.GetMalfunctions(adminToken);
            malfunctionsResponse.Malfunctions.Where(malfunction => malfunction.Description == malfunctionMessage).Should().BeEmpty();
        }
    }
}