using AuthServerJWT.Core.Configuration;
using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.Interfaces;
using AuthServerJWT.Core.models;
using AuthServerJWT.Core.Repositories;
using AuthServerJWT.Core.Services;
using AuthServerJWT.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerJWT.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;
        public AuthenticationService(UserManager<UserApp> userManager, ITokenService tokenService, IOptions<List<Client>> clients, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _clients = clients.Value;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if(loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong",400,  true);

            if(!await _userManager.CheckPasswordAsync(user,loginDto.Password)) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            var token = _tokenService.CreateToken(user);
            var refreshToken = await _userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (refreshToken == null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken { UserId = user.Id, RefreshToken = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
                    
            }
            else
            {
                refreshToken.RefreshToken = token.RefreshToken;
                refreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);


        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client= _clients.SingleOrDefault(x=>x.ClientId == clientLoginDto.ClientId && x.Secret==clientLoginDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("clientid or clientSecret not found", 404, true);
            }
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var isExistrefreshToken = await _userRefreshTokenRepository.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();
            if (isExistrefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh token not found",4040,true);
            }

            var user = await _userManager.FindByIdAsync(isExistrefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User not found", 4040, true);
            }

            var tokenDto= _tokenService.CreateToken(user);
            isExistrefreshToken.RefreshToken = tokenDto.RefreshToken;
            isExistrefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken= await _userRefreshTokenRepository.Where(x=>x.RefreshToken==refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return Response<NoDataDto>.Fail("Refresh token not found", 4040, true);
            }

            _userRefreshTokenRepository.Remove(existRefreshToken);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
