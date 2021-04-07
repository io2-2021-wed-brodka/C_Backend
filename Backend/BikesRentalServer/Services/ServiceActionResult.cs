namespace BikesRentalServer.Services
{
    public class ServiceActionResult<T>
    {
        public string Message { get; set; }
        public T Object { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Success,
        EntityNotFound,
        InvalidStatus,
    }
}
