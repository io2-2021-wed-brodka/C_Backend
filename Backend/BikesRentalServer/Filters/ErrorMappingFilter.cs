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
            if (!(context.Result is ObjectResult result) || (result.StatusCode >= 400 && result.StatusCode < 500))
                return;

            result.Value = new Error
            {
                Message = result.Value as string,
            };
        }
    }
}
