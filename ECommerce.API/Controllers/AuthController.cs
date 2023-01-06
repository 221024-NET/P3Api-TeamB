using System.Data.SqlClient;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;

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
    [EnableCors("_GithubAppP3TB")]
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
            ActionResult result = Ok();

            if (request.email == null || request.password == null || request.firstName == null || request.lastName == null)
            {
                return BadRequest(new { error = "All fields required" });
            }

            User newUser = new User(request.firstName, request.lastName, request.email, request.password);
            bool success = await _context.CreateNewUser(newUser);

            if (!success)
            {
                result = BadRequest("Email already exists");
                return result;
            }

            newUser = await _context.GetUserByEmailAndPassword(newUser.email, newUser.password);
            result = Ok(newUser);
            return result;
        }

        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.email == null || request.password == null)
            {
                return BadRequest(new { error = "Email and password required" });
            }

            var user = await _context.GetUserByEmailAndPassword(request.email.Trim(), request.password.Trim());

            if (user == null)
            {
                return Unauthorized(new { error = "Invalid login information" });
            }
            else
            {
                return Ok(user);
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
