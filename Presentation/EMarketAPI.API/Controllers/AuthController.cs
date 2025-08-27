using EMarketAPI.Application.DTOs;
using EMarketAPI.Persistence.Concretes.Services;
using Microsoft.AspNetCore.Mvc;
 // Core katmanındaki DTO'ları import et

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request.Email);
        return Ok("Eğer e-posta sistemde varsa, sıfırlama linki gönderildi.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            // Burada token ve yeni şifreyi gönderiyoruz
            await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            return Ok("Şifre sıfırlandı.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
    