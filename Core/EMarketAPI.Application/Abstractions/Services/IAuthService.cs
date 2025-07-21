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
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task <string> LoginAsync(LoginDto loginDto);  //jwt token döner.
    }
}
