using System;
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

        private IWebElement GetStationElement(string stationName)
        {
            return driver.FindElement(By.Id($"station-{stationName}"));
        }
    }
}