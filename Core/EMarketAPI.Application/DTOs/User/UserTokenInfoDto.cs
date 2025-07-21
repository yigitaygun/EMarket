using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs.User
{
    public class UserTokenInfoDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}   
