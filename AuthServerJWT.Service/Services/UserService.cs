using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.models;
using AuthServerJWT.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerJWT.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user= new UserApp { Email=createUserDto.Email,UserName=createUserDto.UserName};

            var result= await _userManager.CreateAsync(user,createUserDto.Password);

            if(!result.Succeeded)
            {
                var errors=result.Errors.Select(x=>x.Description).ToList();
                return Response<UserAppDto>.Fail( 400, new ErrorDto(errors, true));
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);

        }

        public async Task<Response<NoContent>> CreateUserRoles(string userName)
        {

            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                await _roleManager.CreateAsync(new() { Name = "admin" });
                await _roleManager.CreateAsync(new() { Name = "manager" });
            }

            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.AddToRoleAsync(user, "admin");
            await _userManager.AddToRoleAsync(user, "manager");


            return Response<NoContent>.Success(StatusCodes.Status201Created);

        }

        public async Task<Response<UserAppDto>> GetUserByAsync(string userName)
        {
            var user= await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Response<UserAppDto>.Fail( "Username not found",404, true);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
