using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesRentalServer.WebApi.Dtos.Requests
{
    public class AddMalfunctionRequest
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
