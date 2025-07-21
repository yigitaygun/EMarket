using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EMarketAPI.Persistence.Identity
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; } = "Customer";
        public ICollection<Order>? Orders { get; set; }
    }
}
