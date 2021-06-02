using OpenQA.Selenium;

namespace SeleniumTests2.Pages
{
    public class AdminUsersPage : AdminSite
    {
        public AdminUsersPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        public bool IsUserBlocked(string username)
        {
            try
            {
                GetUnblockUserButton(username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserUnblocked(string username)
        {
            try
            {
                GetBlockUserButton(username);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void BlockUser(string username)
        {
            GetBlockUserButton(username).Click();
            driver.Sleep();
        }

        public void UnblockUser(string username)
        {
            GetUnblockUserButton(username).Click();
            driver.Sleep();
        }

        private void AssertPage()
        {
            GetUsersHeader();
        }

        private IWebElement GetUsersHeader()
        {
            return driver.FindElement(By.Id("users-header"));
        }

        private IWebElement GetBlockUserButton(string username)
        {
            return driver.FindElement(By.Id($"block-{username}"));
        }

        private IWebElement GetUnblockUserButton(string username)
        {
            return driver.FindElement(By.Id($"unblock-{username}"));
        }
    }
}
