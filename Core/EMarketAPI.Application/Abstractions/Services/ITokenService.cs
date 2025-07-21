using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.DTOs.User;

namespace EMarketAPI.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateToken(UserTokenInfoDto usertoken);
    }
}
