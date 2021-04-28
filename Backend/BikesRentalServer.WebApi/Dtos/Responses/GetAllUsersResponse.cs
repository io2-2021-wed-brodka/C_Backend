using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetAllUsersResponse
    {
        public IEnumerable<GetUserResponse> Users { get; set; }
    }
}
