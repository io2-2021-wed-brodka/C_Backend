using System.Collections.Generic;

namespace BikesRentalServer.Dtos.Responses
{
    public class GetAllUsersResponse
    {
        public IEnumerable<GetUserResponse> Users { get; set; }
    }
}
