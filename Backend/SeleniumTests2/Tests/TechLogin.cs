using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class TechLogin : TestsBase
    {
        public TechLogin(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TechLogInWithWrongCredentialsShouldFail()
        {
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn("some-non-existing-tech", "with-password");

            Action checkIfLoginSucceed = () => new StationsPage(Driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public void TechLogInWithAdminCredentialsShouldSucceed()
        {
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn("admin", "admin");

            Action checkIfLoginSucceed = () => new StationsPage(Driver);
            checkIfLoginSucceed();
        }

        [Fact]
        public async Task TechLogInWithCorrectCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "23456";
            var adminToken = await Api.LogInAsAdmin();
            await Api.AddTech(login, password, adminToken);
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn(login, password);
            
            var stationsPage = new StationsPage(Driver);
            stationsPage.GoToTechMalfunctions();
        }

        [Fact]
        public async Task TechLogOutShouldSucceed()
        {
            var stationsPage = await LoginAsSomeTech();

            var loginPage = stationsPage.LogOut();
        }
    }
}