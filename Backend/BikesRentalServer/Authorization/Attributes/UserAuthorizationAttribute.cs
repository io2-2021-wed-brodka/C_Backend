using BikesRentalServer.Models;

namespace BikesRentalServer.Authorization.Attributes
{
    public class UserAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.User;
    }
}
