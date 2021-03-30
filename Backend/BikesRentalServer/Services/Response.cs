namespace BikesRentalServer.Services
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Object { get; set; }
    }
}
