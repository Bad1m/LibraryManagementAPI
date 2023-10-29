using AutoMapper;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel userModel)
        {
            var user = _mapper.Map<UserDto>(userModel);
            var response = await _userService.Register(user);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateViewModel userViewModel)
        {
            var authRequest = _mapper.Map<AuthenticateRequest>(userViewModel);
            var response = await _userService.Authenticate(authRequest);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userViews = _mapper.Map<List<UserViewModel>>(users);
            return Ok(userViews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userView = _mapper.Map<UserViewModel>(user);
            return userView is not null ? Ok(userView) : NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserViewModel request)
        {
            var userView = _mapper.Map<UserDto>(request);
            var updatedUser = await _userService.UpdateUserAsync(id, userView);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserViewModel>(updatedUser));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
        {
            var isDelete = await _userService.DeleteUserAsync(id);
            return isDelete ? Ok() : BadRequest();
        }
    }
}
