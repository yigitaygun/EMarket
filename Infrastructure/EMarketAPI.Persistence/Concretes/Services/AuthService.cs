using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.DTOs.User;
using EMarketAPI.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMarketAPI.Persistence.Concretes.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mailService = mailService;
        }

        

        public async Task<RegisterResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                UserName=registerDto.UserName,
                Email=registerDto.Email,
            };

            var result=await _userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded)
            {
                return new RegisterResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            var token = await GenerateJwtToken(user);

            return new RegisterResult
            {
                Succeeded = true,
                Token = token,
            };
            
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user=await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted)
                throw new Exception("Sisteme ait kullanıcı bulunamadı.");

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

        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            var result=await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync(bool includeDeleted = false) ////Tüm kullanıcıları (veya sadece aktif kullanıcıları) listele.
        {
            var query = includeDeleted
                ? _userManager.Users
                : _userManager.Users.Where(u => !u.IsDeleted);

            return await query
                .Select(u => new UserSummaryDto
                {
                    Id = u.Id,
                    UserName = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    IsDeleted = u.IsDeleted,
                    Balance = u.Balance,
                })
                .ToListAsync();
        }


        public async Task<UserDetailDto?> GetUserByIdAsync(string userId)  //Tek bir kullanıcı detayını getir.
        {
            var u=await _userManager.Users
                .FirstOrDefaultAsync(x=>x.Id == userId);

            if (u == null)
                return null;

            var roles = await _userManager.GetRolesAsync(u);

            return new UserDetailDto
            {
                Id=u.Id,
                UserName=u.UserName!,
                Email=u.Email!,
                IsDeleted=u.IsDeleted,
                Roles=roles,
                Balance=u.Balance,
                

            };   
        }

        public async Task<bool> RestoreUserByIdAsync(string userId)  //Yanlışlıkla silinen kullanıcıyı geri getir (IsDeleted = false).
        {
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsDeleted)
                return false;

            user.IsDeleted = false;
            var result=await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return; // güvenlik sebebiyle e-posta varsa da yoksa da aynı davran

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var link = $"https://localhost:3000/reset-password?email={email}&token={encodedToken}";

            var body = $"<p>Şifreni sıfırlamak için <a href=\"{link}\">buraya tıkla</a>.</p>";

            await _mailService.SendAsync(email, "Şifre Sıfırlama", body);
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }
    }
}
