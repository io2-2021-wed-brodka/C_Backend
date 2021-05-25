using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetUnblockedBikesCount()
        {
            return GetUnblockedBikes().Count();
        }

        public void ClickOnStation(string stationName)
        {
            // This should change bike station list to opened if it was closed before or vice versa.
            GetStationElement(stationName).Click();
            driver.Sleep();
        }

        public void AddBikeToOpenedStation()
        {
            GetAddBikeButton().Click();
            driver.Sleep();
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

        private IWebElement GetAddBikeButton()
        {
            return driver.FindElement(By.Id("add-bike"));
        }

        private IEnumerable<IWebElement> GetUnblockedBikes()
        {
            return driver.FindElements(By.Id("block-bike"));
        }

        private List<IWebElement> GetBlockedBikes()
        {
            return driver.FindElements(By.Id("unblock-bike")).ToList();
        }
    }
}