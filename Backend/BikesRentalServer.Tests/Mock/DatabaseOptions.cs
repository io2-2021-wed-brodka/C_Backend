using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

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
