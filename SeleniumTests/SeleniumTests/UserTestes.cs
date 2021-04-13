using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
namespace SeleniumTests
{
    public class UserTests
    {
        readonly string _login = "test";
        readonly string _password = "test";
        readonly string _url = "http://localhost:3001";
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
        public void VerifyRentalsDisplay()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > header.MuiAppBar-colorPrimary.MuiAppBar-positionSticky.MuiAppBar-root.MuiPaper-elevation4.MuiPaper-root > div.MuiPaper-elevation0.MuiPaper-root.makeStyles-root-7 > div.MuiTabs-root > div.MuiTabs-fixed.MuiTabs-scroller > div.MuiTabs-centered.MuiTabs-flexContainer")
                );
            navbar.FindElement(By.CssSelector("a:nth-child(2)")).Click();
            Assert.AreEqual(webDriver.Url, _url + "/rentals");
            webDriver.Close();
        }
        [Test]
        public void VerifyStationsDisplay()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > header.MuiAppBar-colorPrimary.MuiAppBar-positionSticky.MuiAppBar-root.MuiPaper-elevation4.MuiPaper-root > div.MuiPaper-elevation0.MuiPaper-root.makeStyles-root-7 > div.MuiTabs-root > div.MuiTabs-fixed.MuiTabs-scroller > div.MuiTabs-centered.MuiTabs-flexContainer")
                );
            navbar.FindElement(By.CssSelector("a:nth-child(2)")).Click();
            Assert.AreEqual(webDriver.Url, _url + "/stations");
            webDriver.Close();
        }
        [Test]
        public void VerifyRentingBike()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > header.MuiAppBar-colorPrimary.MuiAppBar-positionSticky.MuiAppBar-root.MuiPaper-elevation4.MuiPaper-root > div.MuiPaper-elevation0.MuiPaper-root.makeStyles-root-7 > div.MuiTabs-root > div.MuiTabs-fixed.MuiTabs-scroller > div.MuiTabs-centered.MuiTabs-flexContainer")
                );
            navbar.FindElement(By.CssSelector("a:nth-child(2)")).Click();
            var rentalList = webDriver.FindElements(
                By.CssSelector("#root > div.makeStyles-root-1 > div.MuiContainer-maxWidthMd.MuiContainer-root.makeStyles-container-4 > div.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded > ul.MuiList-dense.MuiList-padding.MuiList-root")
                );
            int rentalcount = 0;
            if (rentalList != null && rentalList.Count != 0)
                rentalcount = rentalList[0].FindElements(By.TagName("li")).Count;
            navbar.FindElement(By.CssSelector("a:nth-child(1)")).Click();
            var station = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > div.MuiContainer-maxWidthMd.MuiContainer-root.makeStyles-container-4 > div.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded:nth-child(1) > div.MuiAccordionSummary-root.MuiButtonBase-root")
                );
            station.Click();
            webDriver.FindElement(By.ClassName("MuiButton-containedPrimary")).Click();
            navbar.FindElement(By.CssSelector("a:nth-child(2)")).Click();
            rentalList = webDriver.FindElements(
                By.CssSelector("#root > div.makeStyles-root-1 > div.MuiContainer-maxWidthMd.MuiContainer-root.makeStyles-container-4 > div.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded > ul.MuiList-dense.MuiList-padding.MuiList-root")
                );
            int newRentalCount = 0;
            if (rentalList != null && rentalList.Count != 0)
                newRentalCount = rentalList[0].FindElements(By.TagName("li")).Count;
            Assert.AreEqual(newRentalCount, rentalcount+1);
            webDriver.Close();
        }
        [Test]
        public void VerifyRentingAndReturningBike()
        {
            IWebDriver webDriver = new FirefoxDriver(firefoxOptions);
            Login(webDriver);
            var navbar = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > header.MuiAppBar-colorPrimary.MuiAppBar-positionSticky.MuiAppBar-root.MuiPaper-elevation4.MuiPaper-root > div.MuiPaper-elevation0.MuiPaper-root.makeStyles-root-7 > div.MuiTabs-root > div.MuiTabs-fixed.MuiTabs-scroller > div.MuiTabs-centered.MuiTabs-flexContainer")
                );
            var station = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > div.MuiContainer-maxWidthMd.MuiContainer-root.makeStyles-container-4 > div.MuiAccordion-root.MuiAccordion-rounded.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded:nth-child(1) > div.MuiAccordionSummary-root.MuiButtonBase-root")
                );
            station.Click();
            
            webDriver.FindElement(By.ClassName("MuiButton-containedPrimary")).Click();
            navbar.FindElement(By.CssSelector("a:nth-child(2)")).Click();
            var rentedBikes = webDriver.FindElement(
                By.CssSelector("#root > div.makeStyles-root-1 > div.MuiContainer-maxWidthMd.MuiContainer-root.makeStyles-container-4 > div.MuiPaper-elevation1.MuiPaper-root.MuiPaper-rounded > ul.MuiList-dense.MuiList-padding.MuiList-root")
                );
            rentedBikes.FindElement(By.CssSelector("button:nth-child(2)")).Click();
            var popupStations = webDriver.FindElement(
                By.CssSelector("body > div.MuiDialog-root > div.MuiDialog-container.MuiDialog-scrollPaper > div.MuiDialog-paper.MuiDialog-paperScrollPaper.MuiDialog-paperWidthSm.MuiPaper-elevation24.MuiPaper-root.MuiPaper-rounded > ul.MuiList-padding.MuiList-root")
                );
            popupStations.FindElement(By.CssSelector("div:nth-child(1)")).Click();
            Assert.Pass();
        }
    }
}