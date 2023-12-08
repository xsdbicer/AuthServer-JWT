using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServerJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return ActionResultInstances(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstances(await _userService.GetUserByAsync(HttpContext.User.Identity.Name));
        }



        [HttpPost("CreateUserRoles/{userName}")]
        public async Task<IActionResult> CreateUserRoles(string userName)
        {

            return ActionResultInstances(await _userService.CreateUserRoles(userName));

        }
    }
}
