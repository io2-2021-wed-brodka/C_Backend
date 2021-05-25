using System;
using OpenQA.Selenium;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Xunit.Abstractions;
using System.Reflection;
using System.Linq;

namespace SeleniumTests2
{
    public abstract class TestsBase : IDisposable
    {
        public static readonly string UserSiteUrl = "http://localhost:3001";
        public static readonly string AdminSiteUrl = "http://localhost:3002";
        
        protected RestClient Api { get; set; }
        protected IWebDriver Driver { get; set; }
        
        private static bool _warmedUp;

        protected TestsBase(ITestOutputHelper output)
        {
            Api = new RestClient("http://localhost:8080");
            Api.UseNewtonsoftJson();
            
            Driver = SeleniumHelpers.InitDriverWithUserPage();

            SetTestNameInTabTitle(output);

            // warp up database, so that migration can happen and all tests will run quickly
            if (!_warmedUp)
            {
                Api.LogInBlocking("login", "password");
                _warmedUp = true;
            }
        }

        public void Dispose()
        {
            Driver.Quit();
        }

        protected string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        protected AdminStationsPage LoginAsAdmin()
        {
            var loginPage = new LoginPage(Driver, true);

            loginPage.LogIn("admin", "admin");

            return new AdminStationsPage(Driver);
        }

        protected async Task<StationsPage> LoginAsSomeUser()
        {
            var login = GetUniqueString();
            const string password = "23456";
            await Api.SignUp(login, password);
            var loginPage = new LoginPage(Driver);

            loginPage.LogIn(login, password);

            return new StationsPage(Driver);
        }

        private void SetTestNameInTabTitle(ITestOutputHelper output)
        {
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);
            var displayName = string.Join('.', test.DisplayName.Split('.').Skip(2).ToArray());
            Driver.SetTabTitle(displayName);
        }
    }
}
