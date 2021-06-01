using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class LoginPage : PageBase
    {
        public LoginPage(IWebDriver driver, bool isAdminPage = false) : base(driver)
        {
            AssertPage(isAdminPage);
        }

        private void AssertPage(bool isAdminPage)
        {
            GetLoginInput();
            GetPasswordInput();
            if(!isAdminPage)
            {
                GetSignUpLink();
            }
        }

        public void LogIn(string login, string password)
        {
            GetLoginInput().SendKeys(login);
            GetPasswordInput().SendKeys(password);
            GetSignInButton().Click();
            driver.Sleep();
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
