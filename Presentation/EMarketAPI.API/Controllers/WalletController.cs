using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EMarketAPI.Persistence.Context;
using EMarketAPI.Persistence.Identity;
using EMarketAPI.Application.DTOs.Wallet;

namespace EMarketAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly UserManager<AppUser> _userManager;

        public WalletController(AppDbContext ctx, UserManager<AppUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyBalance()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var user = await _ctx.Users.FirstAsync(u => u.Id == userId);
            return Ok(new { balance = user.Balance });
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Geçersiz istek." });

            if (dto.Amount <= 0)
                return BadRequest(new { message = "Yatırılacak miktar 0’dan büyük olmalıdır." });

            var userId = _userManager.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var user = await _ctx.Users.FirstAsync(u => u.Id == userId);
            user.Balance += dto.Amount;

            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();

            return Ok(new { balance = user.Balance });
        }
    }
}
