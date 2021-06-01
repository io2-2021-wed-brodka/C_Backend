using System;
using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetMalfunctionResponse
    {
        public string Id { get; set; }
        public string BikeId { get; set; }
        public string Description { get; set; }
        public string ReportingUserId { get; set; }
    }
}
