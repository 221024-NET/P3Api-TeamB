using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ECommerce.Data;
using ECommerce.Models;
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
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IContext _context;

        public AuthController(IContext context)
        {
            _context = context;
        }

        [Route("auth/register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            ActionResult result = Ok();

            // Check for invalid email format
            var regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            var isValid = regex.IsMatch(request.email);
            if (!isValid)
            {
                return BadRequest("Invalid email format");
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
            var user = await _context.GetUserByEmailAndPassword(request.email.Trim(), request.password.Trim());

            if (user == null)
            {
                return BadRequest("Invalid login information");
            }
            else
            {
                return Ok(user);
            }
        }

        [Route("auth/reset-password")]
        [HttpPatch]
        public async Task<ActionResult<User>> ResetPassword([FromBody] UserDTO LR)
        {
            try
            {
                return Ok(await _context.UpdateUserPassword(LR.password, LR.email));
            }
            catch
            {
                return BadRequest();
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
