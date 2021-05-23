using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class LoginPage : PageBase
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetLoginInput();
            GetPasswordInput();
            GetSignUpLink();
        }

        public void LogIn(string login, string password)
        {
            GetLoginInput().SendKeys(login);
            GetPasswordInput().SendKeys(password);
            GetSignInButton().Click();
        }

        public SignUpPage GoToSignUp()
        {
            GetSignUpLink().Click();
            return new SignUpPage(driver);
        }

        private IWebElement GetLoginInput()
        {
            return driver.FindElement(By.Id("login"));
        }

        private IWebElement GetPasswordInput()
        {
            return driver.FindElement(By.Id("password"));
        }

        private IWebElement GetSignUpLink()
        {
            return driver.FindElement(By.Id("go-to-sign-up"));
        }

        private IWebElement GetSignInButton()
        {
            return driver.FindElement(By.CssSelector("button"));
        }
    }
}
