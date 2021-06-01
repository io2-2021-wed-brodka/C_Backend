using OpenQA.Selenium;

namespace SeleniumTests2
{
    public abstract class PageBase
    {
        public static string DriverIsOnWrongPage = "Driver is on wrong page";
        protected readonly IWebDriver driver;

        public PageBase(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool ContainsSnackbar()
        {
            try 
            {
                driver.FindElement(By.ClassName("MuiSnackbar-root"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
