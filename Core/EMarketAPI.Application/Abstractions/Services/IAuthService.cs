using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using EMarketAPI.Application.DTOs.User;

namespace EMarketAPI.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<RegisterResult> RegisterAsync(RegisterDto registerDto);
        Task <string> LoginAsync(LoginDto loginDto);  //jwt token döner.
        Task<bool> DeleteUserByIdAsync(string userId);

        Task<IEnumerable<UserSummaryDto>> GetAllUsersAsync(bool includeDeleted = false);
        Task<UserDetailDto?> GetUserByIdAsync(string userId);
        Task<bool> RestoreUserByIdAsync(string userId);

    }
}
