using BikesRentalServer.Authorization.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikesRentalServer.Filters
{
    public class SwaggerAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizationAttribute>();

            if (!attributes.Any())
                return;
            
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized",
            });
            operation.Responses.Add("403", new OpenApiResponse
            {
                Description = "Forbidden",
            });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer Auth",
                                Type = ReferenceType.SecurityScheme
                            },
                        },
                        Array.Empty<string>()
                    },
                },
            };
        }
    }
}
