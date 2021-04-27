using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetAllStationsResponse
    {
        public IEnumerable<GetStationResponse> Stations { get; set; }
    }
}
