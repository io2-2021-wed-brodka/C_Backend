using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikesRentalServer.Models;
namespace BikesRentalServer.Dtos.Responses
{
    public class GetBikeResponse
    {
        public string id { get; set; }
        public Station station { get; set; }
        public User user { get; set; }

    }
}
