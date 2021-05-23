using OpenQA.Selenium;
using System.Linq;
using System.Threading;

namespace SeleniumTests2
{
    public static class SeleniumHelpers
    {
        public static void OpenAdminTab(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchToAdminTab();
            driver.Navigate().GoToUrl(TestsBase.AdminSiteUrl);
            Thread.Sleep(500);
        }

        public static void SwitchToUserTab(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }

        public static void SwitchToAdminTab(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static void Sleep(this IWebDriver driver, int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public static void Sleep(this IWebDriver driver)
        {
            driver.Sleep(1);
        }
    }
}
