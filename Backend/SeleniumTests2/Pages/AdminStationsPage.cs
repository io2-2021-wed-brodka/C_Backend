using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class AdminStationsPage : AdminSite
    {
        public AdminStationsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetStationsHeader();
            GetNewStationNameInput();
            GetNewStationSubmitButton();
        }

        public void AddStation(string stationName)
        {
            GetNewStationNameInput().SendKeys(stationName);
            GetNewStationSubmitButton().Click();
            driver.Sleep();
        }

        public bool HasStation(string stationName)
        {
            try
            {
                GetStationElement(stationName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IWebElement GetStationsHeader()
        {
            return driver.FindElement(By.Id("stations-header"));
        }

        private IWebElement GetNewStationNameInput()
        {
            return driver.FindElement(By.Id("new-station-name-input"));
        }

        private IWebElement GetNewStationSubmitButton()
        {
            return driver.FindElement(By.Id("new-station-submit-button"));
        }

        private IWebElement GetStationElement(string stationName)
        {
            return driver.FindElement(By.Id($"station-{stationName}"));
        }
    }
}