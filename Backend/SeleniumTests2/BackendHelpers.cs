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

        public static async Task<string> LogInAsAdmin(this RestClient client)
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

        public static Task<GetStationResponse> AddStation(this RestClient client, string stationName, string adminToken)
        {
            var body = new AddStationRequest
            {
                Name = stationName
            };
            
            return client.PostRequest<GetStationResponse>("stations", body, adminToken);
        }

        public static Task<GetStationResponse> BlockStation(this RestClient client, string stationId, string adminToken)
        {
            var body = new BlockStationRequest
            {
                Id = stationId
            };
            
            return client.PostRequest<GetStationResponse>("stations/blocked", body, adminToken);
        }

        public static Task<GetBikeResponse> AddBike(this RestClient client, string stationId, string adminToken)
        {
            var body = new AddBikeRequest
            {
                StationId = stationId
            };
            
            return client.PostRequest<GetBikeResponse>("bikes", body, adminToken);
        }

        public static Task<GetBikeResponse> RentBike(this RestClient client, string bikeId, string adminToken)
        {
            var body = new RentBikeRequest
            {
                Id = bikeId
            };

            return client.PostRequest<GetBikeResponse>("bikes/rented", body, adminToken);
        }

        public static Task<GetReservedBikeResponse> ReserveBike(this RestClient client, string bikeId, string adminToken)
        {
            var body = new ReserveBikeRequest
            {
                Id = bikeId
            };

            return client.PostRequest<GetReservedBikeResponse>("bikes/reserved", body, adminToken);
        }

        public static Task<GetBikeResponse> BlockBike(this RestClient client, string bikeId, string adminToken)
        {
            var body = new BlockBikeRequest
            {
                Id = bikeId
            };

            return client.PostRequest<GetBikeResponse>("bikes/blocked", body, adminToken);
        }

        public static Task<GetUserResponse> BlockUser(this RestClient client, string UserId, string adminToken)
        {
            var body = new BlockUserRequest
            {
                Id = UserId
            };

            return client.PostRequest<GetUserResponse>("users/blocked", body, adminToken);
        }

        public static Task<GetAllUsersResponse> GetUsers(this RestClient client, string adminToken)
        {
            return client.GetRequest<GetAllUsersResponse>("users", adminToken);
        }

        public static async Task<string> GetUserId(this RestClient client, string UserName, string adminToken)
        {
            var users = await client.GetUsers(adminToken);
            foreach (var user in users.Users)
                if (user.Name == UserName) return user.Id;
            return "";
        }
    }
}
