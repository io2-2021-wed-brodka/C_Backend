using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface ITechsService
    {
        ServiceActionResult<Malfunction> RemoveMalfunction(string malfunctionId);
    }
}
