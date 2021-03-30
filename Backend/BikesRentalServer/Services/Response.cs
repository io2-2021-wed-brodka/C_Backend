using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesRentalServer.Services
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Object { get; set; }
    }
}
