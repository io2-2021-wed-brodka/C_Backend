using System;
using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class UserSite : PageBase
    {
        public UserSite(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            driver.FindElement(By.Id("user-site-navbar"));
        }

        protected string GetSelectedTabHref()
        {
            return driver.FindElement(By.CssSelector(".Mui-selected.MuiTab-root"))
                .GetAttribute("href");
        }
    }
}
