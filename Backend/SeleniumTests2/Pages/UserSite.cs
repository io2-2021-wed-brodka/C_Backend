using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class UserSite : PageBase
    {
        public UserSite(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetNavbar();
            GetAccountMenu();
        }

        public LoginPage LogOut()
        {
            GetAccountMenu().Click();
            driver.Sleep();
            GetLogOutButton().Click();
            driver.Sleep();
            return new LoginPage(driver);
        }

        protected string GetSelectedTabHref()
        {
            return driver.FindElement(By.CssSelector(".Mui-selected.MuiTab-root"))
                .GetAttribute("href");
        }

        private IWebElement GetAccountMenu()
        {
            return driver.FindElement(By.Id("account-menu"));
        }

        private IWebElement GetLogOutButton()
        {
            return driver.FindElement(By.Id("logout"));
        }

        private IWebElement GetNavbar()
        {
            return driver.FindElement(By.Id("user-site-navbar"));
        }

        public RentalsPage GoToRentals()
        {
            driver.FindElement(By.Id("rentals")).Click();
            driver.Sleep();

            return new RentalsPage(driver);
        }

        public StationsPage GoToStations()
        {
            driver.FindElement(By.Id("stations")).Click();
            driver.Sleep();

            return new StationsPage(driver);
        }
    }
}
