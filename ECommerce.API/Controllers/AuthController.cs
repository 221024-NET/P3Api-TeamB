using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;

namespace ECommerce.API.Controllers
{
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
        public async Task<ActionResult> Register([FromBody] User newUser)
        {
            // _logger.LogInformation("auth/register triggered");
            var response = await _context.CreateNewUser(newUser);
            if (response == null)
            {
                return BadRequest("User already exists");
            }
            return CreatedAtAction(nameof(_context.GetUserById), new { id = newUser.Id });
            //Ok(await _repo.CreateNewUserAndReturnUserIdAsync(newUser));
            // _logger.LogInformation("auth/register completed successfully");
            // _logger.LogWarning("auth/register completed with errors");
        }

        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserDTO LR)
        {
            // _logger.LogInformation("auth/login triggered");
            var response = await _context.GetUserLogin(LR.password, LR.email);

            if (response == null)
            {
                return NotFound("Invalid login info");
            }

            return Ok(response);
            //Ok(await _repo.GetUserLoginAsync(LR.password, LR.email));
            // _logger.LogInformation("auth/login completed successfully");
        }

        [Route("auth/logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            // _logger.LogInformation("auth/logout triggered");
            // _logger.LogInformation("auth/logout completed successfully");
            return Ok();
        }
    }
}
