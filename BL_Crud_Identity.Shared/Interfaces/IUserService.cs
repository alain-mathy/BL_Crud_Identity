using BL_Crud_Identity.Shared.DTOs.Auth;
using BL_Crud_Identity.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL_Crud_Identity.Shared.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user with their profile information.
        /// </summary>
        /// <param name="dto">The registration data transfer object.</param>
        /// <returns>A list of errors if registration fails; otherwise, an empty collection.</returns>
        Task<IEnumerable<string>> RegisterUserAsync(RegisterDto dto);

        /// <summary>
        /// Authenticates a user based on their login credentials.
        /// </summary>
        /// <param name="dto">The login data transfer object.</param>
        /// <returns>True if authentication succeeded; otherwise, false.</returns>
        Task<bool> LoginUserAsync(LoginDto dto);

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a specific user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        Task<UserDto?> GetUserByIdAsync(string id);

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="dto">The data transfer object containing updated details.</param>
        Task<bool> UpdateUserAsync(UserUpdateDto dto);

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        Task<bool> DeleteUserAsync(string id);
    }
}
