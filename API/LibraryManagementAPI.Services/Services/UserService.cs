using AutoMapper;
using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using LibraryManagementAPI.Services.Constants;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IJWTService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserValidator _userDtoValidator;
        public UserService(IUserRepository userRepository, IJWTService tokenService, IMapper mapper, IUserValidator userDtoValidator)
        {
            _repository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _userDtoValidator = userDtoValidator;
        }
        public async Task<AuthenticateResponse> RegisterAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            _userDtoValidator.ValidateUser(userDto);
            if (!await IsUsernameUniqueAsync(cancellationToken, userDto.Username))
            {
                throw new DbUpdateException(UserErrors.UsernameAlreadyExists);
            }

            var user = _mapper.Map<User>(userDto);
            user.Password = HashPassword(user.Password);
            _repository.Insert(user);
            await _repository.SaveChangesAsync(cancellationToken);

            var response = await AuthenticateAsync(new AuthenticateRequest
            {
                Username = user.Username,
                Password = userDto.Password
            }, cancellationToken);
            return response;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByUsernameAsync(model.Username, cancellationToken);
            if (user is null)
            {
                throw new InvalidOperationException(UserErrors.UserNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                throw new UnauthorizedAccessException(UserErrors.InvalidPassword);
            }

            var token = _tokenService.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntity = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureUserExists(userEntity);
            var user = _mapper.Map<UserDto>(userEntity);
            return user;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var userEntities = await _repository.GetAsync(cancellationToken);
            var users = _mapper.Map<List<UserDto>>(userEntities);
            return users;
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserDto user, CancellationToken cancellationToken)
        {
            _userDtoValidator.ValidateUser(user);
            var userEntity = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureUserExists(userEntity);
            if (userEntity.Username != user.Username && !await IsUsernameUniqueAsync(cancellationToken, user.Username,id))
            {
                throw new DbUpdateException(UserErrors.UsernameAlreadyExists);
            }

            userEntity = _mapper.Map<User>(user);
            userEntity.Id = id;
            _repository.Update(userEntity);
            await _repository.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureUserExists(user);
            _repository.Delete(new User { Id = id });
            await _repository.SaveChangesAsync(cancellationToken);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private void EnsureUserExists(User? existingUser)
        {
            if (existingUser is null)
            {
                throw new InvalidOperationException(UserErrors.UserNotFound);
            }
        }

        private async Task<bool> IsUsernameUniqueAsync(CancellationToken cancellationToken, string username, int? userId = null)
        {
            var existingUser = await _repository.GetUserByUsernameAsync(username, cancellationToken);
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
    }
}