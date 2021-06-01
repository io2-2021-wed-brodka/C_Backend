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

        public void CancelReservation(string bikeId)
        {
            // to_do
            driver.Sleep();
        }

        public void RentBike(string bikeId)
        {
            GetRentButton(bikeId).Click();
            driver.Sleep();
        }

        public bool HasBike(string bikeId)
        {
            try
            {
                GetRentButton(bikeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IWebElement GetRentButton(string bikeId)
        {
            return driver.FindElement(By.Id($"rent-{bikeId}"));
        }
    }
}