using OpenQA.Selenium;
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

        public void AddStation(string stationName, int limit = 0)
        {
            GetNewStationNameInput().SendKeys(stationName);
            if(limit > 0){
                GetNewStationLimitInput().SendKeys(limit.ToString());
            }
            GetNewStationSubmitButton().Click();
            driver.Sleep(2);
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
            driver.Sleep(2);
        }

        public void UnblockBike(string bikeId)
        {
            GetUnblockBikeButton(bikeId).Click();
            driver.Sleep(2);
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
            driver.Sleep(2);
        }

        public void ClickRemoveBike(string bikeId)
        {
            GetRemoveBikeButton(bikeId).Click();
            driver.Sleep();
        }

        public void ClickRemoveStation(string stationName)
        {
            GetRemoveStationButton(stationName).Click();
            driver.Sleep(2);
        }

        public void ClickBlockStation(string stationName)
        {
            GetBlockStationButton(stationName).Click();
            driver.Sleep(2);
        }

        public void ClickUnblockStation(string stationName)
        {
            GetUnblockStationButton(stationName).Click();
            driver.Sleep(2);
        }

        public int GetActiveBikesCount(string stationName)
        {
            return int.Parse(
                driver.FindElement(By.Id($"chip-active-{stationName}"))
                    .GetAttribute("data-count"));
        }

        public int GetBrokenBikesCount(string stationName)
        {
            return int.Parse(
                driver.FindElement(By.Id($"chip-malfunctions-{stationName}"))
                    .GetAttribute("data-count"));
        }

        public int GetReservedBikesCount(string stationName)
        {
            return int.Parse(
                driver.FindElement(By.Id($"chip-reserved-{stationName}"))
                    .GetAttribute("data-count"));
        }

        private IWebElement GetStationsHeader()
        {
            return driver.FindElement(By.Id("stations-header"));
        }

        private IWebElement GetNewStationNameInput()
        {
            return driver.FindElement(By.Id("new-station-name-input"));
        }

        private IWebElement GetNewStationLimitInput()
        {
            return driver.FindElement(By.Id("new-station-bikes-limit-input"));
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

        private IWebElement GetBlockStationButton(string stationName)
        {
            return driver.FindElement(By.Id($"block-station-{stationName}"));
        }

        private IWebElement GetUnblockStationButton(string stationName)
        {
            return driver.FindElement(By.Id($"unblock-station-{stationName}"));
        }

        private IWebElement GetRemoveStationButton(string stationName)
        {
            return driver.FindElement(By.Id($"remove-station-{stationName}"));
        }
    }
}