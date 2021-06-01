using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class TechBikesPage : UserSite
    {
        public TechBikesPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
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

        public void RemoveBike(string bikeId)
        {
            GetRemoveBikeButton(bikeId).Click();
            driver.Sleep();
        }

        private void AssertPage()
        {
            GetBikesHeader();
        }

        private IWebElement GetBikesHeader()
        {
            return driver.FindElement(By.Id("bikes-header"));
        }
        
        private IWebElement GetBikeElement(string bikeId)
        {
            return driver.FindElement(By.Id($"bike-{bikeId}"));
        }

        private IWebElement GetBlockBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"block-{bikeId}"));
        }

        private IWebElement GetUnblockBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"unblock-{bikeId}"));
        }

        private IWebElement GetRemoveBikeButton(string bikeId)
        {
            return driver.FindElement(By.Id($"remove-{bikeId}"));
        }
    }
}
