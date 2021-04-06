using BikesRentalServer.Authorization.Attributes;
using BikesRentalServer.DataAccess;
using BikesRentalServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikesRentalServer.Authorization
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
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized");
                return;
            }
            
            var username = Encoding.UTF8.GetString(Convert.FromBase64String(token.ToString()));
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username);
            var roles = GetAuthorizedRoles(context.ActionDescriptor as ControllerActionDescriptor);
            
            if (user is null)
            {
                if (roles.Any())
                    context.Result = new UnauthorizedObjectResult("Unauthorized");
                
                return;
            }

            if (!roles.Contains(user.Role))
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
