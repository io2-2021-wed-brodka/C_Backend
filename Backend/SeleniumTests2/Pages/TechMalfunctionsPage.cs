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
    }
}
