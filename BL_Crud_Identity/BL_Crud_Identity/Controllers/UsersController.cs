using BL_Crud_Identity.Data;
using BL_Crud_Identity.Shared.DTOs.Auth;
using BL_Crud_Identity.Shared.DTOs.User;
using BL_Crud_Identity.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BL_Crud_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        public UsersController(IUserService _userService)
        {
            this._userService = _userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken] // Allow anonymous access for registration
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var errors = await _userService.RegisterUserAsync(dto);
            var errorList = errors.ToList();
            if (errorList.Any())
            {
                return BadRequest(errorList);
            }
            return Ok();
        }

        /// <summary>
        /// Logs in a user and returns an authentication token.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous] // Anyone can attempt to login
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid login credentials.");
            }

            // 1. Check if the password is correct without triggering response headers premature locking
            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid login credentials.");
            }

            // 2. Perform the actual SignIn which is natively designed to handle cookie attachment safe for Blazor App pipelines
            await signInManager.SignInAsync(user, isPersistent: dto.RememberMe);

            return Ok();
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Updates a user's profile.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateUserAsync(dto);
            if (!result)
            {
                return BadRequest("Failed to update user profile.");
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete user.");
            }
            return NoContent();
        }
    }
}
