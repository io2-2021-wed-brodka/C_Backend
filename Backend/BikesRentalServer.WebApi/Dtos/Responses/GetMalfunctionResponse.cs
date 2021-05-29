using System;
using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetMalfunctionResponse
    {
        public string Id { get; set; }
        public GetBikeResponse Bike { get; set; }
        public DateTime DetectionDate { get; set; }
        public GetUserResponse ReportingUser { get; set; }
        public string Description { get; set; }
        public MalfunctionState MalfunctionState { get; set; }
    }
}