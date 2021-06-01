using System.Collections.Generic;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetAllTechsResponse
    {
        public IEnumerable<GetTechResponse> Techs { get; set; }
    }
}
