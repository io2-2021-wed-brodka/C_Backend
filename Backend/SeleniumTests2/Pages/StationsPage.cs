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
    }
}