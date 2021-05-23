using System;
using Xunit;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using RestSharp;

namespace SeleniumTests2
{
    public abstract class TestsBase : IDisposable
    {
        private readonly string _url = "http://localhost:3001";
        protected readonly RestClient _api = new RestClient("http://localhost:8080");
        protected IWebDriver _driver;

        protected TestsBase()
        {
            var chromeOptions = new ChromeOptions();
            _driver = new RemoteWebDriver(chromeOptions);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_url);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        protected string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
