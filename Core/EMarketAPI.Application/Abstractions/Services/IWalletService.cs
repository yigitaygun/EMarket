using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.Abstractions.Services
{
    public interface IWalletService
    {
        Task<decimal> GetBalanceAsync(string userId);
        Task DebitAsync(string userId,decimal amount); // bakiyeden düş
        Task CreditAsync(string userId,decimal amount); // iade/ekleme
    }
}
