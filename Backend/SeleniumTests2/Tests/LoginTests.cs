using System;
using Xunit;
using System.Threading;
using FluentAssertions;
using System.Threading.Tasks;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SeleniumTests2
{
    public class LoginTests : TestsBase
    {
        [Fact]
        public void LogInWithWrongCredentialsShouldFail()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn("some-non-existing-user", "with-password");
            Thread.Sleep(1000);

            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            checkIfLoginSucceed.Should().Throw<Exception>();
        }

        [Fact]
        public void LogInWithAdminCredentialsShouldSucceed()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn("admin", "admin");
            Thread.Sleep(1000);
            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            
            checkIfLoginSucceed();
        }


        [Fact]
        public async Task LogInWithCorrectCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await _api.SignUp(login, password);
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn(login, password);
            Thread.Sleep(1000);
            Action checkIfLoginSucceed = () => new StationsPage(_driver);
            
            checkIfLoginSucceed();
        }
        
    }
}
