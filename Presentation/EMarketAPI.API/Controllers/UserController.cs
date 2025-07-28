using System.Security.Claims;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMarketAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task <IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token=await _authService.LoginAsync(loginDto);
            if(token == null)
            {
                return Unauthorized("Geçersiiz kullanıcı veya şifre");
            }
            return Ok (new { token });
        }

        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result=await _authService.RegisterAsync(registerDto);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { token = result.Token });
        }



        [HttpDelete("{id}")]

        public async Task <IActionResult> DeleteUser(string id)
        {
            var ok=await _authService.DeleteUserByIdAsync(id);
            if (!ok) return NotFound("Kullanıcı bulunamadı veya zaten silinmiş.");
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var users = await _authService.GetAllUsersAsync(includeDeleted);
            return Ok(users);
        }



        // Tek kullanıcıyı getir
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(user);
        }

        // Soft-delete edilmiş kullanıcıyı geri getir
        [HttpPost("restore/{id}")]
        
        public async Task<IActionResult> RestoreUser(string id)
        {
            var ok = await _authService.RestoreUserByIdAsync(id);
            if (!ok) return NotFound("Kullanıcı bulunamadı veya zaten aktif.");
            return Ok("Kullanıcı geri getirildi.");
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }




    }
}
