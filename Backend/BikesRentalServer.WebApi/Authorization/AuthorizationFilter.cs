using BikesRentalServer.DataAccess;
using BikesRentalServer.Infrastructure;
using BikesRentalServer.Models;
using BikesRentalServer.WebApi.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikesRentalServer.WebApi.Authorization
{
    public class AuthorizationFilter : IActionFilter
    {
        private readonly UserContext _userContext;
        private readonly DatabaseContext _dbContext;

        public AuthorizationFilter(UserContext userContext, DatabaseContext dbContext)
        {
            _userContext = userContext;
            _dbContext = dbContext;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var roles = GetAuthorizedRoles(context.ActionDescriptor as ControllerActionDescriptor).ToArray();
            
            // no roles = allow anonymous
            if (!roles.Any())
                return;
            
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }

            var username = Encoding.UTF8.GetString(Convert.FromBase64String(token.ToString().Replace("Bearer ", "")));
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username);
            
            if (user is null || !roles.Contains(user.Role))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }
            
            _userContext.SetOnce(username, user.Role);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static IEnumerable<UserRole> GetAuthorizedRoles(ControllerActionDescriptor descriptor)
        {
            return descriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizationAttribute), true).Select(x => ((AuthorizationAttribute)x).Role);
        }
    }
}
