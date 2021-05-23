using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikesRentalServer.Models;

namespace BikesRentalServer.Services.Abstract
{
    public interface IMalfunctionsService
    {
        #region Basics
        ServiceActionResult<Malfunction> AddMalfunction(string bikeId, string description);
        ServiceActionResult<Malfunction> GetMalfunction(string bike);

        #endregion
    }
}
