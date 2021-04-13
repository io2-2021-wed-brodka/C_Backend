using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
namespace SeleniumTests
{
    public class UserTests
    {
        string _login = "test";
        string _password = "test";
        string _path = "http://localhost:3001";
        FirefoxOptions firefoxOptions;
       
        [SetUp]
        public void Setup()
        {
            firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;
        }

        [Test]
        public void VerifyLoginAsValidUser()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl(_path);
            webDriver.FindElement(By.Id("login")).SendKeys(_login);
            webDriver.FindElement(By.Id("password")).SendKeys(_login);
            webDriver.FindElement(By.CssSelector("button")).Click();

            string expectedApplicationBarText = "CityBikes";
            var applicationBarText = webDriver.FindElement(By.CssSelector("#root > div.makeStyles-root-1 > header.MuiAppBar-colorPrimary.MuiAppBar-positionSticky.MuiAppBar-root.MuiPaper-elevation4.MuiPaper-root > div.MuiToolbar-gutters.MuiToolbar-regular.MuiToolbar-root > h6.MuiTypography-h6.MuiTypography-root.makeStyles-title-6")).Text;
            Assert.AreEqual(applicationBarText, expectedApplicationBarText);
            webDriver.Close();
        }
        
    }
}