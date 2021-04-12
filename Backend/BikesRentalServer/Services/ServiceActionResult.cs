namespace BikesRentalServer.Services
{
    public class ServiceActionResult<T>
    {
        public string Message { get; private set; }
        public T Object { get; private set; }
        public Status Status { get; private set; }

        public static ServiceActionResult<T> Success(T result)
        {
            return new ServiceActionResult<T>
            {
                Object = result,
                Status = Status.Success
            };
        }

        public static ServiceActionResult<T> EntityNotFound(string message)
        {
            return new ServiceActionResult<T>
            {
                Status = Status.EntityNotFound,
                Message = message
            };
        }

        public static ServiceActionResult<T> InvalidState(string message)
        {
            return new ServiceActionResult<T>
            {
                Status = Status.InvalidState,
                Message = message
            };
        }
    }

    public enum Status
    {
        Success,
        EntityNotFound,
        InvalidState,
    }
}
