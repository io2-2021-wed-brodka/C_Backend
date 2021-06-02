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

        public void CloseSnackbar()
        {
            try
            {
                driver.FindElement(By.Id("close-snackbar")).Click();
                driver.Sleep();
            }
            catch {}
        }

        public ReservationsPage GoToReservations()
        {
            driver.FindElement(By.Id("reservations")).Click();
            driver.Sleep();

            return new ReservationsPage(driver);
        }

        public StationsPage GoToStations()
        {
            driver.FindElement(By.Id("stations")).Click();
            driver.Sleep();

            return new StationsPage(driver);
        }

        public TechBikesPage GoToTechBikes()
        {
            driver.FindElement(By.Id("bikes")).Click();
            driver.Sleep();

            return new TechBikesPage(driver);
        }

        public TechMalfunctionsPage GoToTechMalfunctions()
        {
            driver.FindElement(By.Id("malfunctions")).Click();
            driver.Sleep();

            return new TechMalfunctionsPage(driver);
        }

        public bool AreTechTabsVisible()
        {
            try
            {
                driver.FindElement(By.Id("malfunctions"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
