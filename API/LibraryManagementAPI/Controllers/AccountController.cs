using LibraryManagementAPI.Filters;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [TypeFilter(typeof(CancellationTokenFilter))]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto user, CancellationToken cancellationToken)
        {
            var response = await _userService.RegisterAsync(user, cancellationToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest authRequest, CancellationToken cancellationToken)
        {
            var response = await _userService.AuthenticateAsync(authRequest, cancellationToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PagedResult<UserDto>> GetAllUsers(CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);

            var totalUsers = users.Count;
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            var paginatedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pagedResult = new PagedResult<UserDto>
            {
                Items = paginatedUsers,
                TotalItems = totalUsers,
                Page = page,
                TotalPages = totalPages
            };
            return pagedResult;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserDto request, CancellationToken cancellationToken)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request, cancellationToken);
            return Ok(updatedUser);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return Ok();
        }
    }
}