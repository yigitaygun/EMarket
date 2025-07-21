using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.DTOs.User;
using EMarketAPI.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMarketAPI.Persistence.Concretes.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                UserName=registerDto.UserName,
                Email=registerDto.Email,
            };

            var result=await _userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception($"Kayıt başarısız:");
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            var token = await GenerateJwtToken(user);
            return token;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user=await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new Exception("Email Adresi Hatalı.");

            var result=await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if (!result.Succeeded)
                throw new Exception("Şifre Hatalı.");

            var token = await GenerateJwtToken(user);
            return token;
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var tokenInfo = new UserTokenInfoDto
            {
                UserId = user.Id,
                Email = user.Email!,
                Roles = roles.ToList()
            };

            return _tokenService.CreateToken(tokenInfo);
        }
    }
}
