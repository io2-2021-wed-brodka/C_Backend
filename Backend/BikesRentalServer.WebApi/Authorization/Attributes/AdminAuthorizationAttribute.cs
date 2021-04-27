using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Authorization.Attributes
{
    public class AdminAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.Admin;
    }
}
