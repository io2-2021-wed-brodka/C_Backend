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
        public static async Task<LogInResponse> LogIn(this RestClient client, string login, string password)
        {
            var body = new LogInRequest
            {
                Login = login,
                Password = password
            };
            
            return await client.PostRequest<LogInResponse>("login", body);
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
