using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BikesRentalServer.WebApi.Dtos.Responses
{
    public class GetTechResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
