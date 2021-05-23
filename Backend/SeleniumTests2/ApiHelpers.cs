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
    public static class ApiHelpers
    {
        public static async Task<T> PostRequest<T>(this RestClient client, string endpoint, object body)
        {
            var request = new RestRequest(endpoint, DataFormat.Json);
            request.AddJsonBody(body);

            var resp = await client.ExecutePostAsync<T>(request);

            return resp.Data;
        }

        public static Task<T> PostRequest<T>(this RestClient client, string endpoint, object body, string token)
        {
            var request = new RestRequest(endpoint, DataFormat.Json);
            request.AddJsonBody(body);
            request.AddHeader("Authorization", $"Bearer {token}");

            return client.PostAsync<T>(request);
        }
    }
}
