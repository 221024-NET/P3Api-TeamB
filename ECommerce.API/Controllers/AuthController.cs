using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;

namespace ECommerce.API.Controllers
{
    public class LoginRequest
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

    public class RegisterRequest
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }

    [ApiController]
    public class AuthController : ControllerBase
    {
        /*** old ADO stuff Kept in comments for reference ***/
        // private readonly IRepository _repo;
        private readonly ILogger<AuthController> _logger;
        private readonly IContext _context;

        //public AuthController(IRepository repo, ILogger<AuthController> logger)
        //{
        //    this._repo = repo;
        //    this._logger = logger;
        //}
        public AuthController(IContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("auth/register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request.email == null || request.password == null || request.firstName == null || request.lastName == null)
            {
                return BadRequest(new { error = "All fields required" });
            }

            User newUser = new User(request.firstName, request.lastName, request.email, request.password);

            var response = await _context.CreateNewUser(newUser);

            if (response == null)
            {
                return BadRequest("Email already exists");
            }

            return CreatedAtAction(nameof(_context.GetUserById), new { id = newUser.Id });
        }

        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.email == null || request.password == null)
            {
                return BadRequest(new { error = "Email and password required" });
            }

            var userExists = await _context.GetUserLogin(request.email, request.password);

            if (userExists)
            {
                return Ok();
            }
            else
            {
                return Unauthorized(new { error = "Invalid login information" });
            }
        }

        [Route("auth/logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            return Ok();
        }
    }
}
