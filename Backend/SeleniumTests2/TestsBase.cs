using System;
using Xunit;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace SeleniumTests2
{
    public abstract class TestsBase : IDisposable
    {
        public static readonly string UserSiteUrl = "http://localhost:3001";
        public static readonly string AdminSiteUrl = "http://localhost:3002";
        protected readonly RestClient _api;
        protected IWebDriver _driver;
        private static bool _warmedUp;

        protected TestsBase()
        {
            _api = new RestClient("http://localhost:8080");
            _api.UseNewtonsoftJson();
            
            var chromeOptions = new ChromeOptions();
            _driver = new RemoteWebDriver(chromeOptions);
            _driver.Manage().Window.Maximize();

            _driver.Navigate().GoToUrl(UserSiteUrl);

            // warp up database, so that migration can happen and all tests will run quickly
            if(!_warmedUp)
            {
                _api.LogInBlocking("login", "password");
                _warmedUp = true;
            }
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        protected string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        protected AdminStationsPage LoginAsAdmin()
        {
            var loginPage = new LoginPage(_driver, true);

            loginPage.LogIn("admin", "admin");

            return new AdminStationsPage(_driver);
        }

        protected async Task<StationsPage> LoginAsSomeUser()
        {
            var login = GetUniqueString();
            var password = "23456";
            var token = await _api.SignUp(login, password);
            var loginPage = new LoginPage(_driver);

            loginPage.LogIn(login, password);

            return new StationsPage(_driver);
        }
    }
}
