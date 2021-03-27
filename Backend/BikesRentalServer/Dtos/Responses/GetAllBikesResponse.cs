using System.Collections.Generic;

namespace BikesRentalServer.Dtos.Responses
{
    public class GetAllBikesResponse
    {
        public IEnumerable<GetBikeResponse> Bikes { get; set; }
    }
}
