using System;
using Xunit;
using FluentAssertions;

namespace SeleniumTests2
{
    public class SignUpTests : TestsBase
    {
        [Fact]
        public void SignUpWithExistingLoginShouldFail()
        {
            var loginPage = new LoginPage(_driver);
            var signUpPage = loginPage.GoToSignUp();

            signUpPage.SignUp("admin", "some pass");

            Action checkIfSignUpSucceed = () => new StationsPage(_driver);
            checkIfSignUpSucceed.Should().Throw<Exception>();
            signUpPage.ContainsSnackbar().Should().BeTrue();
        }

        [Fact]
        public void SignUpWithUniqueCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "123";

            var loginPage = new LoginPage(_driver);
            var signUpPage = loginPage.GoToSignUp();

            signUpPage.SignUp(login, password);
            Action checkIfSignUpSucceed = () => new StationsPage(_driver);
            
            checkIfSignUpSucceed();
        }
    }
}
