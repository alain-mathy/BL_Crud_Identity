using BL_Crud_Identity.Data;
using BL_Crud_Identity.Shared.DTOs.Auth;
using BL_Crud_Identity.Shared.DTOs.User;
using BL_Crud_Identity.Shared.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BL_Crud_Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> RegisterUserAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                ZipCode = dto.ZipCode,
                City = dto.City
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Enumerable.Empty<string>();
            }

            return result.Errors.Select(e => e.Description);
        }

        /// <inheritdoc />
        public async Task<bool> LoginUserAsync(LoginDto dto)
        {
            // First check if the user exists and the password is correct
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid) return false;

            // Sign in using the sign-in manager which handles cookie creation
            var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                dto.RememberMe,
                lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Street = user.Street,
                    ZipCode = user.ZipCode,
                    City = user.City
                })
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                ZipCode = user.ZipCode,
                City = user.City
            };
        }

        /// <inheritdoc />
        public async Task<bool> UpdateUserAsync(UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Street = dto.Street;
            user.ZipCode = dto.ZipCode;
            user.City = dto.City;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
