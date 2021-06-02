using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class TechMalfunctionsPage : UserSite
    {
        public TechMalfunctionsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetMalfunctionsHeader();
        }

        private IWebElement GetMalfunctionsHeader()
        {
            return driver.FindElement(By.Id("malfunctions-header"));
        }

        public bool HasMalfunction(string malfunctionId)
        {
            try
            {
                GetRemoveMalfunctionButton(malfunctionId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveMalfunction(string malfunctionId)
        {
            GetRemoveMalfunctionButton(malfunctionId).Click();
            driver.Sleep();
        }

        private IWebElement GetRemoveMalfunctionButton(string malfunctionId)
        {
            return driver.FindElement(By.Id($"remove-{malfunctionId}"));
        }
    }
}
