using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

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
            _logger.LogInformation("auth/register triggered");
            try
            {
                return CreatedAtAction(nameof(_context.GetUserById), new {id = newUser.Id});
                    //Ok(await _repo.CreateNewUserAndReturnUserIdAsync(newUser));
                _logger.LogInformation("auth/register completed successfully");
            }
            catch
            {
                return BadRequest();
                _logger.LogWarning("auth/register completed with errors");
            }
        }


        [Route("auth/login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] UserDTO LR)
        {
            _logger.LogInformation("auth/login triggered");
            try
            {
                return Ok(await _context.GetUserLogin(LR.password, LR.email));
                    
                    //Ok(await _repo.GetUserLoginAsync(LR.password, LR.email));
                _logger.LogInformation("auth/login completed successfully");
            }
            catch
            {
                return BadRequest();
                _logger.LogWarning("auth/login completed with errors");
            }
        }

        [Route("auth/reset-password")]
        [HttpPatch]
        public async Task<ActionResult<User>> resetPassword([FromBody] UserDTO LR)
        {
            _logger.LogInformation("auth/reset-password triggered");
            try
            {
                _logger.LogInformation("auth/reset-password executed successfully");
                return Ok(await _context.UpdateUserPassword(LR.password, LR.email));
            }
            catch
            {
                _logger.LogWarning("auth/reset-password completed with errors");
                return BadRequest();
            }
        }

        [Route("auth/logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            _logger.LogInformation("auth/logout triggered");
            return Ok();
            _logger.LogInformation("auth/logout completed successfully");
        }

    }
}
