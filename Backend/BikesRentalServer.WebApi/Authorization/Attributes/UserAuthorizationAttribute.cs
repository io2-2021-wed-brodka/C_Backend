using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Authorization.Attributes
{
    public class UserAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.User;
    }
}
