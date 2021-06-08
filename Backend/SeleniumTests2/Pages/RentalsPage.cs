using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class RentalsPage : UserSite
    {
        public RentalsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            if(!GetSelectedTabHref().EndsWith("/rentals"))
            {
                throw new Exception(DriverIsOnWrongPage);
            }
        }

        public void ReturnBike(string bikeId, string stationId)
        {
            CloseSnackbar();
            driver.FindElement(By.Id($"return-{bikeId}")).Click();
            driver.Sleep();
            driver.FindElement(By.Id($"return-on-station-{stationId}")).Click();
            driver.Sleep();
        }

        public bool HasBike(string bikeId)
        {
            try
            {
                driver.FindElement(By.Id($"return-{bikeId}"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ReportMalfunction(string bikeId, string malfunctionMessage, string stationId)
        {
            driver.FindElement(By.Id($"report-{bikeId}")).Click();
            driver.Sleep();
            driver.FindElement(By.Id("malfunction-description")).SendKeys(malfunctionMessage);
            driver.FindElement(By.Id("report-malfunction")).Click();
            driver.Sleep();
            driver.FindElement(By.Id($"return-on-station-{stationId}")).Click();
            driver.Sleep();
        }
    }
}