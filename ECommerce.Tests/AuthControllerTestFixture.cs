using System;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Tests
{
    public class AuthControllerTestFixture : IDisposable
    {
        public Context Context { get; private set; }

        public AuthControllerTestFixture()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<AuthControllerTests>()
                .Build();

            string connString = config.GetConnectionString("ecommDB");

            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connString)
                .Options;

            Context = new Context(options);

        }

        public void Dispose()
        {
            // Clean up the test data here
            Context.Dispose();
        }
    }
}
