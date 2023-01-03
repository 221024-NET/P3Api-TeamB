// using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ECommerce.API.Controllers;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ECommerce.Tests
{
    public class AuthControllerTests : IClassFixture<AuthControllerTestFixture>
    {
        private readonly IContext _context;

        public AuthControllerTests(AuthControllerTestFixture fixture)
        {
            _context = fixture.Context;

            var config = new ConfigurationBuilder()
                .AddUserSecrets<AuthControllerTests>()
                .Build();

            string connString = config.GetConnectionString("ecommDB");

            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connString)
                .Options;

            _context = new Context(options);
        }

        [Fact]
        public async Task Test_Register_ReturnsOk_OnSuccess()
        {
            // Arrange
            string randomString = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[+/]", "");
            string email = $"{randomString}@example.com";

            var request = new RegisterRequest
            {
                firstName = "Test",
                lastName = "User",
                email = email,
                password = "password"
            };
            var controller = new AuthController(_context);

            // Act
            var result = await controller.Register(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            // New test case:
            var request2 = new RegisterRequest
            {
                firstName = "Test",
                lastName = "User",
                email = email, // Assume this email already exists in the database
                password = "password"
            };
            var result2 = await controller.Register(request2);
            Assert.IsType<BadRequestObjectResult>(result2);
            Assert.Equal("Email already exists", ((BadRequestObjectResult)result2).Value);
        }

        [Fact]
        public async Task Test_Login_ReturnsOk_OnSuccess()
        {
            // Arrange
            var request = new LoginRequest
            {
                email = "brianna@gmail.com",
                password = "brianna"
            };

            var controller = new AuthController(_context);

            // Act
            var result = await controller.Login(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Test_Login_ReturnsBadRequest_OnInvalidCredentials()
        {
            // Arrange
            string randomString = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[+/]", "");
            string email = $"{randomString}@example.com";

            var request = new LoginRequest
            {
                email = email,
                password = "incorrect"
            };
            var controller = new AuthController(_context);

            // Act
            var result = await controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid login information", ((BadRequestObjectResult)result).Value);
        }
    }
}
