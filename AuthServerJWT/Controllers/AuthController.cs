using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServerJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //api/auth/
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);

            return ActionResultInstances(result);
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateTokenByClient(clientLoginDto);

            return ActionResultInstances(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);

            return ActionResultInstances(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)

        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);

            return ActionResultInstances(result);
        }
    }
}
