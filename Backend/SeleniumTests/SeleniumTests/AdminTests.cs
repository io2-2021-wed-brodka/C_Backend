using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTests
{
    class AdminTests
    {
        readonly string _login = "admin";
        readonly string _password = "admin";
        readonly string _url = "http://localhost:3002";
        FirefoxOptions firefoxOptions;
        private void Login(IWebDriver webDriver)
        {
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl(_url);
            webDriver.FindElement(By.Id("login")).SendKeys(_login);
            webDriver.FindElement(By.Id("password")).SendKeys(_password);
            webDriver.FindElement(By.CssSelector("button")).Click();
        }

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
            webDriver.Navigate().GoToUrl(_url);
            webDriver.FindElement(By.Id("login")).SendKeys(_login);
            webDriver.FindElement(By.Id("password")).SendKeys(_password);
            webDriver.FindElement(By.CssSelector("button")).Click();
            Assert.AreEqual(webDriver.Url, _url + "/stations");
            webDriver.Close();
        }

        [Test]
        public void VerifyBikesDisplay()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(By.CssSelector("ul a:nth-child(2)"));
            navbar.Click();
            Assert.AreEqual(webDriver.Url, _url + "/bikes");
            webDriver.Close();
        }

        [Test]
        public void VerifyUsersDisplay()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(By.CssSelector("ul a:nth-child(3)"));
            navbar.Click();
            Assert.AreEqual(webDriver.Url, _url + "/users");
            webDriver.Close();
        }

        [Test]
        public void VerifyTechsDisplay()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(By.CssSelector("ul a:nth-child(4)"));
            navbar.Click();
            Assert.AreEqual(webDriver.Url, _url + "/techs");
            webDriver.Close();
        }

        [Test]
        public void VerifyLogoutAsAdmin()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(By.CssSelector("ul div:nth-child(5)"));
            navbar.Click();
            Assert.AreEqual(webDriver.Url, _url + "/login");
            webDriver.Close();
        }


    }
}
