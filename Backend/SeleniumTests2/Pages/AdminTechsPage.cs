using OpenQA.Selenium;

namespace SeleniumTests2
{
    public class AdminTechsPage : AdminSite
    {
        public AdminTechsPage(IWebDriver driver) : base(driver)
        {
            AssertPage();
        }

        private void AssertPage()
        {
            GetTechsHeader();
        }

        public void AddTech(string login, string password)
        {
            GetNewTechNameInput().SendKeys(login);
            GetNewTechPasswordInput().SendKeys(password);
            GetNewTechSubmitButton().Click();
            driver.Sleep();
        }

        public bool HasTech(string techName)
        {
            try
            {
                GetRemoveTechButton(techName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ClickRemoveTech(string techName)
        {
            GetRemoveTechButton(techName).Click();
            driver.Sleep();
        }

        private IWebElement GetTechsHeader()
        {
            return driver.FindElement(By.Id("techs-header"));
        }

        private IWebElement GetNewTechNameInput()
        {
            return driver.FindElement(By.Id("new-tech-name-input"));
        }

        private IWebElement GetNewTechPasswordInput()
        {
            return driver.FindElement(By.Id("new-tech-password-input"));
        }

        private IWebElement GetNewTechSubmitButton()
        {
            return driver.FindElement(By.Id("new-tech-submit-button"));
        }

        private IWebElement GetRemoveTechButton(string techName)
        {
            return driver.FindElement(By.Id($"remove-{techName}"));
        }
    }
}