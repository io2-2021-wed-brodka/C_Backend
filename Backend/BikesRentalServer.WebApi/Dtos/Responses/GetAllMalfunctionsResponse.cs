using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetAllMalfunctionsResponse
    {
        public IEnumerable<GetMalfunctionResponse> Malfunctions { get; set; }
    }
}
