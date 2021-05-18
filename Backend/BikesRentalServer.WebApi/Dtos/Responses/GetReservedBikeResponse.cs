using System;
using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetReservedBikeResponse
    {
        public string Id { get; set; }
        public GetStationResponse Station { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ReservedTill { get; set; }
    }
}
