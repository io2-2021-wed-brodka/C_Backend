using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class ReservationsPage : UserSite
    {
        public ReservationsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            if(!GetSelectedTabHref().EndsWith("/reservations"))
            {
                throw new Exception(DriverIsOnWrongPage);
            }
        }

        public void RentBike(string bikeId)
        {
            GetRentBikeButton(bikeId).Click();
            driver.Sleep();
        }

        public void CancelBike(string bikeId)
        {
            GetCancelButton(bikeId).Click();
            driver.Sleep();
        }

        public bool HasBike(string bikeId)
        {
            try
            {
                GetRentBikeButton(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IWebElement GetRentBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"rent-{bikeId}"));
        }

        public IWebElement GetCancelButton(string bikeId)
        {
            return driver.FindElement(By.Id($"cancel-{bikeId}"));
        }
    }
}