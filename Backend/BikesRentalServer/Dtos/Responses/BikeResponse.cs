using BikesRentalServer.Models;

namespace BikesRentalServer.Dtos.Responses
{
    public class BikeResponse
    {
        public string Id { get; set; }
        public StationDto Station { get; set; }
        public UserDto User { get; set; }
        public BikeState State { get; set; }

        public class UserDto
        {
            public string Id { get; set; }
            public string Name { get; set; }

        }
        public class StationDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
