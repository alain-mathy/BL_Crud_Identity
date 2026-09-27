using BL_Crud_Identity.Data;
using BL_Crud_Identity.Shared.DTOs.Auth;
using BL_Crud_Identity.Shared.DTOs.User;
using BL_Crud_Identity.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
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
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            // Extract the unique identifier of the currently logged-in account
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            // Security check: Block the request if the user is not an Admin AND is trying to read someone else's data
            if (!isAdmin && currentUserId != id)
            {
                return Forbid(); // Returns HTTP 403 Forbidden safely
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Updates a user's profile.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize] // Only authenticated users can update their profile]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            // Security check: Block the request if the user is not an Admin AND is trying to modify someone else's data
            if (!isAdmin && currentUserId != dto.Id)
            {
                return Forbid(); // Returns HTTP 403 Forbidden safely
            }

            var success = await _userService.UpdateUserAsync(dto);
            if (!success) return BadRequest("Could not update user details.");
            return Ok(success);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete users
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete user.");
            }
            return NoContent();
        }

        /// <summary>
        /// Authenticates the current user session out of the application and clears identity cookies.
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous] // Anyone can request a logout session clear
        public async Task<IActionResult> Logout()
        {
            var signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

            await signInManager.SignOutAsync();

            // Redirect completely back to home page to re-evaluate auth states
            return Redirect("/");
        }
    }
}
