using BikesRentalServer.Models;
using System;

namespace BikesRentalServer.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class AuthorizationAttribute : Attribute
    {
        public abstract UserRole Role { get; }
    }
}
