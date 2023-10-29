using AutoMapper;
using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using LibraryManagementAPI.Data.Settings;
using LibraryManagementAPI.Services.Constants;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementAPI.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserValidator _userDtoValidator;
        private readonly AuthSettings _authSettings;

        public UserService(IUserRepository userRepository, IMapper mapper, IUserValidator userDtoValidator, AuthSettings authSettings)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userDtoValidator = userDtoValidator;
            _authSettings = authSettings;
        }

        public async Task<AuthenticateResponse> Register(UserDto userDto)
        {
            _userDtoValidator.ValidateUser(userDto);

            if (!await IsUsernameUniqueAsync(userDto.Username))
            {
                throw new DbUpdateException(UserErrors.UsernameAlreadyExists);
            }

            var user = _mapper.Map<User>(userDto);
            user.Password = HashPassword(user.Password);
            await _userRepository.InsertAsync(user);

            var response = await Authenticate(new AuthenticateRequest
            {
                Username = user.Username,
                Password = userDto.Password
            });

            return response;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            if (user is null)
            {
                throw new InvalidOperationException(UserErrors.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                throw new UnauthorizedAccessException(UserErrors.InvalidPassword);
            }

            var token = GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var userEntity = await _userRepository.GetByIdAsync(id);

            var user = _mapper.Map<UserDto>(userEntity);

            return user;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var userEntities = await _userRepository.GetAsync();

            var user = _mapper.Map<List<UserDto>>(userEntities);

            return user;
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserDto user)
        {
            _userDtoValidator.ValidateUser(user);

            var userEntity = await _userRepository.GetByIdAsync(id);

            if (userEntity is null)
            {
                throw new InvalidOperationException(UserErrors.UserNotFound);
            }

            if (userEntity.Username != user.Username && !await IsUsernameUniqueAsync(user.Username, user.Id))
            {
                throw new DbUpdateException(UserErrors.UsernameAlreadyExists);
            }

            user.Id = userEntity.Id;
            userEntity = _mapper.Map<User>(user);

            await _userRepository.UpdateAsync(userEntity);
            return user;
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            return _userRepository.DeleteAsync(new User { Id = id });
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private async Task<bool> IsUsernameUniqueAsync(string username, int? userId = null)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser == null)
            {
                return true;
            }

            if (userId.HasValue && existingUser.Id == userId.Value)
            {
                return true; 
            }

            return false;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(_authSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(
              new SymmetricSecurityKey(key),
              SecurityAlgorithms.HmacSha256Signature
              ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}