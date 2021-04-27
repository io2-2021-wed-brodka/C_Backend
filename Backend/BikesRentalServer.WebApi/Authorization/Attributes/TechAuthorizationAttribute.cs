using BikesRentalServer.Models;

namespace BikesRentalServer.WebApi.Authorization.Attributes
{
    public class TechAuthorizationAttribute : AuthorizationAttribute
    {
        public override UserRole Role => UserRole.Tech;
    }
}
