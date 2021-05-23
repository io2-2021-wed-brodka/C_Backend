using System;
using Xunit;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using RestSharp;
using BikesRentalServer.WebApi.Dtos.Responses;
using System.Threading.Tasks;
using BikesRentalServer.WebApi.Dtos.Requests;

namespace SeleniumTests2
{
    public static class BackendHelpers
    {
        public static Task<LogInResponse> LogIn(this RestClient client, string login, string password)
        {
            var body = new LogInRequest
            {
                Login = login,
                Password = password
            };
            
            return client.PostRequest<LogInResponse>("login", body);
        }

        public static async Task<string> LogInAsAdmin(this RestClient client, string login, string password)
        {
            return (await client.LogIn("admin", "admin")).Token;
        }

        // do not use this, it is only for database warmup
        public static void LogInBlocking(this RestClient client, string login, string password)
        {
            var body = new LogInRequest
            {
                Login = login,
                Password = password
            };
            
            client.PostRequestBlocking<LogInResponse>("login", body);
        }

        public static Task<LogInResponse> SignUp(this RestClient client, string login, string password)
        {
            var body = new RegisterRequest
            {
                Login = login,
                Password = password
            };
            
            return client.PostRequest<LogInResponse>("register", body);
        }
    }
}
