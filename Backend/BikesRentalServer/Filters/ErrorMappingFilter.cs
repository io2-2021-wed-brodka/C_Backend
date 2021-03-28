using BikesRentalServer.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BikesRentalServer.Filters
{
    public class ErrorMappingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result && IsClientErrorStatusCode(result.StatusCode))
            {
                result.Value = new Error
                {
                    Message = result.Value as string,
                };
            }
        }

        private static bool IsClientErrorStatusCode(int? statusCode)
        {
            return statusCode is >= 400 and < 500;
        }
    }
}
