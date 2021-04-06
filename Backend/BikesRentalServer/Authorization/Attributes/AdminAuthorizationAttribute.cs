using BikesRentalServer.Models;

namespace BikesRentalServer.Authorization.Attributes
{
    public class AdminAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.Admin;
    }
}
