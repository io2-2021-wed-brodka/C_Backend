using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool HasBike(string bikeId)
        {
            try
            {
                GetBikeElement(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsBikeBlocked(string bikeId)
        {
            try
            {
                GetUnblockBikeButton(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsBikeUnblocked(string bikeId)
        {
            try
            {
                GetBlockBikeButton(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetBikesCount()
        {
            return GetBikeElements().Count;
        }

        public void BlockBike(string bikeId)
        {
            GetBlockBikeButton(bikeId).Click();
            driver.Sleep();
        }

        public void UnblockBike(string bikeId)
        {
            GetUnblockBikeButton(bikeId).Click();
            driver.Sleep();
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

        public void ClickRemoveBike(string bikeId)
        {
            GetRemoveBikeButton(bikeId).Click();
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

        private IWebElement GetBikeElement(string bikeId)
        {
            return driver.FindElement(By.Id($"bike-{bikeId}"));
        }

        private List<IWebElement> GetBikeElements()
        {
            return driver.FindElements(By.CssSelector("[id^=bike-]")).ToList();
        }

        private IWebElement GetBlockBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"block-bike-{bikeId}"));
        }

        private IWebElement GetUnblockBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"unblock-bike-{bikeId}"));
        }

        private IWebElement GetRemoveBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"remove-bike-{bikeId}"));
        }
    }
}