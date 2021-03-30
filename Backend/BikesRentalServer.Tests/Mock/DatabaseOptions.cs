using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesRentalServer.Tests.Mock
{
    public class DatabaseOptions : DbContextOptions
    {
        public DatabaseOptions() : base(null) { }

        public string ConnectionString { get; set; }

        public override Type ContextType => throw new NotImplementedException();

        public override DbContextOptions WithExtension<TExtension>([NotNullAttribute] TExtension extension)
        {
            throw new NotImplementedException();
        }
    }
}
