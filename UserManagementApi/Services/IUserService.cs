using Microsoft.EntityFrameworkCore;
using UserManagementApi.DTOs;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<string> RegisterAsync(CreateUserDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<bool> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
