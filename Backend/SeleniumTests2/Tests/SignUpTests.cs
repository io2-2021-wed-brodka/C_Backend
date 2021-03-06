using System;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class SignUpTests : TestsBase
    {
        public SignUpTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SignUpWithExistingLoginShouldFail()
        {
            var loginPage = new LoginPage(Driver);
            var signUpPage = loginPage.GoToSignUp();

            signUpPage.SignUp("admin", "some pass");

            Action checkIfSignUpSucceed = () => new StationsPage(Driver);
            checkIfSignUpSucceed.Should().Throw<Exception>();
            signUpPage.ContainsSnackbar().Should().BeTrue();
        }

        [Fact]
        public void SignUpWithUniqueCredentialsShouldSucceed()
        {
            var login = GetUniqueString();
            var password = "123";

            var loginPage = new LoginPage(Driver);
            var signUpPage = loginPage.GoToSignUp();

            signUpPage.SignUp(login, password);
            Action checkIfSignUpSucceed = () => new StationsPage(Driver);
            
            checkIfSignUpSucceed();
        }
    }
}
