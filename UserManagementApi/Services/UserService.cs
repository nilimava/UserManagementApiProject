using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementApi.DTOs;
using UserManagementApi.Models;
using UserManagementApi.Repositories;

namespace UserManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository repo, TokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users=await _repo.GetAllAsync();

            return users.Select(user=>
                new UserDto()
                {
                   Id=user.Id,
                   Username=user.Username,
                   Email=user.Email
                });
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user=await _repo.GetByIdAsync(id);

            if(user==null)
            {
                return null;
            }
            return new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<string> RegisterAsync(CreateUserDto dto)
        {
            var existingUser = await _repo.GetByEmailAsync(dto.Email);

            if (existingUser != null)
                return "User already exists";

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repo.AddAsync(user);
            await _repo.SaveAsync();

            return "User registered successfully";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null ||!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return "Invalid credentials";

            return _tokenService.CreateToken(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            //Mapping DTO -> User entity
            user.Username = updateDto.Username;
            user.Email = updateDto.Email;

            _repo.Update(user);
            await _repo.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            _repo.Delete(user);
            await _repo.SaveAsync();

            return true;
        }
    }
}
