using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BikesRentalServer.Dtos.Responses
{
    public class BikeResponse
    {
        public string id { get; set; }
        public Station station { get; set; }
        public User user { get; set; }
        public BikeState state { get; set; }

        public class User
        {
            public string id { get; set; }
            public string name { get; set; }

        }
        public class Station
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}
