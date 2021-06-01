using OpenQA.Selenium;
using SeleniumTests2.Pages;

namespace SeleniumTests2
{
    public class AdminSite : PageBase
    {
        public AdminSite(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            driver.FindElement(By.Id("admin-site"));
        }

        public LoginPage LogOut()
        {
            GetLogOutButton().Click();
            driver.Sleep();
            return new LoginPage(driver, true);
        }

        public AdminStationsPage GoToStations()
        {
            driver.FindElement(By.Id("go-to-stations")).Click();
            driver.Sleep();

            return new AdminStationsPage(driver);
        }

        public AdminBikesPage GoToBikes()
        {
            driver.FindElement(By.Id("go-to-bikes")).Click();
            driver.Sleep();

            return new AdminBikesPage(driver);
        }

        private IWebElement GetLogOutButton()
        {
            return driver.FindElement(By.Id("logout"));
        }
    }
}
