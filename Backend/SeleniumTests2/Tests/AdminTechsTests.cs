using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class AdminTechsTests : TestsBase
    {
        public AdminTechsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void NewTechShouldBeOnList()
        {
            var stationName = GetUniqueString();
            var techLogin = GetUniqueString();
            var techPassword = "124";

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var techsPage = adminStationsPage.GoToTechs();

            techsPage.AddTech(techLogin, techPassword);
            techsPage.HasTech(techLogin).Should().BeTrue();

            Driver.SwitchToUserTab();
            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(techLogin, techPassword);
            var stationsPage = new StationsPage(Driver);
            stationsPage.AreTechTabsVisible().Should().BeTrue();
        }

        [Fact]
        public async Task RemovingTechShouldSucceed()
        {
            var stationName = GetUniqueString();
            var techLogin = GetUniqueString();
            var techPassword = "124";
            var adminToken = await Api.LogInAsAdmin();
            await Api.AddTech(techLogin, techPassword, adminToken);

            Driver.OpenAdminTab();
            var adminStationsPage = LoginAsAdmin();
            var techsPage = adminStationsPage.GoToTechs();
            techsPage.HasTech(techLogin).Should().BeTrue();
            techsPage.ClickRemoveTech(techLogin);
            techsPage.HasTech(techLogin).Should().BeFalse();

            Driver.SwitchToUserTab();
            var loginPage = new LoginPage(Driver);
            loginPage.LogIn(techLogin, techPassword);
            Action checkIfTechCannotLogIn = () => new StationsPage(Driver);
            checkIfTechCannotLogIn.Should().Throw<Exception>();
        }
    }
}
