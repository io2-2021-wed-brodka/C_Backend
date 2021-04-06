using System.Collections.Generic;

namespace BikesRentalServer.Dtos.Responses
{
    public class GetAllStationsResponse
    {
        public IEnumerable<GetStationResponse> Stations { get; set; }
    }
}
