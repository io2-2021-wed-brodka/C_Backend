namespace BikesRentalServer.Services
{
    public class ServiceActionResult<T>
    {
        public string Message { get; }
        public T Object { get; }
        public Status Status { get; }

        public ServiceActionResult(string message, T @object, Status status)
        {
            Message = message;
            Object = @object;
            Status = status;
        }
    }

    public static class ServiceActionResult
    {
        public static ServiceActionResult<T> Success<T>(T result) => new ServiceActionResult<T>(null, result, Status.Success);
        public static ServiceActionResult<T> EntityNotFound<T>(string message) => new ServiceActionResult<T>(message, default, Status.EntityNotFound);
        public static ServiceActionResult<T> InvalidState<T>(string message) => new ServiceActionResult<T>(message, default, Status.InvalidState);
    }

    public enum Status
    {
        Success,
        EntityNotFound,
        InvalidState,
    }
}
