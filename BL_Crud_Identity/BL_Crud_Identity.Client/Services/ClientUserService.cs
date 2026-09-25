using BL_Crud_Identity.Shared.DTOs.Auth;
using BL_Crud_Identity.Shared.DTOs.User;
using BL_Crud_Identity.Shared.Interfaces;
using System.Net.Http.Json;

namespace BL_Crud_Identity.Client.Services
{
    public class ClientUserService : IUserService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientUserService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests.</param>
        public ClientUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> RegisterUserAsync(RegisterDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/register", dto);

            if (response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<string>();
            }

            var errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            return errors ?? new List<string> { "An unknown error occurred during registration." };
        }

        public async Task<bool> LoginUserAsync(LoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/login", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("api/users");
            return users ?? Enumerable.Empty<UserDto>();
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/users", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
