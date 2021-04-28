using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetAllBikesResponse
    {
        public IEnumerable<GetBikeResponse> Bikes { get; set; }
    }
}
