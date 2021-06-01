using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class StationsPage : UserSite
    {
        public StationsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            if(!GetSelectedTabHref().EndsWith("/stations"))
            {
                throw new Exception(DriverIsOnWrongPage);
            }
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

        public void OpenBikesList(string stationName)
        {
            GetStationElement(stationName).Click();
            driver.Sleep();
        }
        
        public bool HasBike(string bikeId)
        {
            try
            {
                GetReserveButton(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public void RentBike(string bikeId)
        {
            GetRentButton(bikeId).Click();
            driver.Sleep();
        }

        public void ReserveBike(string bikeId)
        {
            GetReserveButton(bikeId).Click();
            driver.Sleep();
        }

        public int GetBikesCount()
        {
            return GetBikeElements().Count;
        }

        private IWebElement GetStationElement(string stationName)
        {
            return driver.FindElement(By.Id($"station-{stationName}"));
        }

        private IWebElement GetReserveButton(string bikeId)
        {
            return driver.FindElement(By.Id($"reserve-{bikeId}"));
        }

        private IWebElement GetRentButton(string bikeId)
        {
            return driver.FindElement(By.Id($"rent-{bikeId}"));
        }

        private List<IWebElement> GetBikeElements()
        {
            return driver.FindElements(By.CssSelector("[id^=bike-]")).ToList();
        }
    }
}