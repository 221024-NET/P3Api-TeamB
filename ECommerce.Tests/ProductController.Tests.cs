using System.Threading.Tasks;
using ECommerce.API.Controllers;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Xunit;

namespace ECommerce.Tests
{
    public class ProductControllerTests
    {
        private readonly IContext _context;

        public ProductControllerTests()
        {
            // Build a configuration object
            var config = new ConfigurationBuilder()
                .AddUserSecrets<AuthControllerTests>()
                .Build();

            // Get the connection string from the user secrets
            string connString = config.GetConnectionString("ecommDB");

            // Create an instance of DbContextOptions<Context>
            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connString)
                .Options;

            // Create an instance of IContext to use in the tests
            _context = new Context(options);
        }

    }
}
