using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class SignUpPage : PageBase
    {
        public SignUpPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetLoginInput();
            GetPasswordInput();
            GetSignInLink();
        }

        public void SignUp(string login, string password)
        {
            GetLoginInput().SendKeys(login);
            GetPasswordInput().SendKeys(password);
            GetSignUpButton().Click();
            driver.Sleep();
        }

        public LoginPage GoToSignUp()
        {
            GetSignInLink().Click();
            return new LoginPage(driver);
        }

        private IWebElement GetLoginInput()
        {
            return driver.FindElement(By.Id("login"));
        }

        private IWebElement GetPasswordInput()
        {
            return driver.FindElement(By.Id("password"));
        }

        private IWebElement GetSignInLink()
        {
            return driver.FindElement(By.Id("go-to-sign-in"));
        }

        private IWebElement GetSignUpButton()
        {
            return driver.FindElement(By.CssSelector("button"));
        }
    }
}
