using BikesRentalServer.Models;

namespace BikesRentalServer.Authorization.Attributes
{
    public class TechAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.Tech;
    }
}
