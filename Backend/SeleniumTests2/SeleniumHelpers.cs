using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Linq;
using System.Threading;

namespace SeleniumTests2
{
    public static class SeleniumHelpers
    {
        public static void OpenAdminTab(this IWebDriver driver)
        {
            var testName = driver.FindElement(By.Id("test-helper")).Text;
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchToAdminTab();
            driver.Navigate().GoToUrl(TestsBase.AdminSiteUrl);
            Thread.Sleep(100);
            driver.SetTabTitle(testName);
            Thread.Sleep(100);
        }

        public static IWebDriver InitDriverWithUserPage()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            var driver = new RemoteWebDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(TestsBase.UserSiteUrl);

            return driver;
        }

        public static void SwitchToUserTab(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }

        public static void SwitchToAdminTab(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static void Sleep(this IWebDriver driver, int seconds = 1)
        {
            Thread.Sleep(seconds * 1000);
        }

        public static void SetTabTitle(this IWebDriver driver, string title)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"document.getElementById('test-helper').innerText = \"{TestsBase.TestNo}: {title}\";");
        }
    }
}
