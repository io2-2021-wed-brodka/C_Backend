using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

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
