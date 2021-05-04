using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTests
{
    public class AdminTests
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

        [Test]
        public void VerifyAddingStation()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var stationsCount = webDriver.FindElements(By.CssSelector(".MuiPaper-root.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-rounded")).Count;
            webDriver.FindElement(By.CssSelector("input")).SendKeys("stacja selenium");
            var buttons = webDriver.FindElements(By.CssSelector("button"));
            foreach(var button in buttons)
            {
                if(button.Text == "ADD STATION")
                {
                    button.Click();
                    break;
                }
            }
            int newStationCount = webDriver.FindElements(By.CssSelector(".MuiPaper-root.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-rounded")).Count;
            Assert.AreEqual(stationsCount + 1, newStationCount);

            webDriver.Close();
        }

        [Test]
        public void VerifyAddingBikeToStation()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var station = webDriver.FindElement(By.CssSelector("#root > main.makeStyles-content-12 > div.MuiContainer-maxWidthLg.MuiContainer-root.makeStyles-container-13 > div.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded:nth-child(5) > div.MuiAccordionSummary-root.MuiButtonBase-root"));
            station.Click();
            int bikesCount = webDriver.FindElements(By.CssSelector("li")).Count;
            var buttons = webDriver.FindElements(By.CssSelector("button:nth-child(1)"));
            foreach (var button in buttons)
            {
                if(button.Text == "ADD BIKE")
                {
                    button.Click();
                    break;
                }
            }
            int newBikesCount = webDriver.FindElements(By.CssSelector("li")).Count;
            Assert.AreEqual(bikesCount + 1, newBikesCount);
            webDriver.Close();
        }

        [Test]
        public void VerifyRemoveStation()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var stations = webDriver.FindElements(By.CssSelector(".MuiPaper-root.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-rounded"));
            int stationsCount = stations.Count;
            var station = stations[stations.Count-1];
            station.Click();
            var buttons = webDriver.FindElements(By.CssSelector("button"));
            foreach (var button in buttons)
            {
                if(button.Text == "REMOVE")
                {
                    button.Click();
                    break;
                }
            }
            stations = webDriver.FindElements(By.CssSelector(".MuiPaper-root.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-rounded"));
            int newStationsCount = stations.Count;
            Assert.AreEqual(stationsCount, newStationsCount + 1);
        }

        [Test]
        public void VerifyRemovingBikeFromStation()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var station = webDriver.FindElement(By.CssSelector("#root > main.makeStyles-content-12 > div.MuiContainer-maxWidthLg.MuiContainer-root.makeStyles-container-13 > div.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded:nth-child(5) > div.MuiAccordionSummary-root.MuiButtonBase-root"));
            station.Click();
            int bikesCount = webDriver.FindElements(By.CssSelector("li")).Count;
            var list = webDriver.FindElement(By.CssSelector("ul"));
            var blockButtons = webDriver.FindElements(By.CssSelector("li  button:nth-child(1)"));
            foreach (var button in blockButtons)
            {
                if (button.Text == "BLOCK")
                {
                    button.Click();
                    break;
                }
            }
            var RemoveButtons = webDriver.FindElements(By.CssSelector("li button:nth-child(2)"));
            {
                foreach (var button in RemoveButtons)
                {
                    if (button.Text == "REMOVE")
                    {
                        button.Click();
                        break;
                    }
                }
            }
            int newBikesCount = webDriver.FindElements(By.CssSelector("li")).Count;
            Assert.AreEqual(bikesCount, newBikesCount + 1);
            webDriver.Close();
        }
    }
}
