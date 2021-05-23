using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class AdminSite : PageBase
    {
        public AdminSite(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            driver.FindElement(By.Id("admin-site"));
        }
    }
}