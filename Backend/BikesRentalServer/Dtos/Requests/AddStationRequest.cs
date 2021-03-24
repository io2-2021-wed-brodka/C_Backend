namespace BikesRentalServer.Dtos.Requests
{
    public class AddStationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Active,
            Inactive
        }
    }
}
