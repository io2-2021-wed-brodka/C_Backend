using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface IMalfunctionsService
    {
        ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId);
    }
}
