using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Persistence.Context;
using EMarketAPI.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMarketAPI.Persistence.Concretes.Services
{
    public class WalletService:IWalletService
    {
        private readonly UserManager<AppUser> _users;
        private readonly AppDbContext _context;

        public WalletService(UserManager<AppUser> users, AppDbContext context)
        {
            _users = users;
            _context = context;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        
            =>await _users.Users.Where(u => u.Id == userId).Select(u => u.Balance).FirstAsync();

        public async Task DebitAsync(string userId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be >0");
            using var tx=await _context.Database.BeginTransactionAsync();


            var user =await _users.Users.FirstAsync(u => u.Id == userId);

            if (user.Balance < amount)
                throw new ArgumentException("yetersiz bakiye");

            user.Balance-=amount;

            var ok=await _users.UpdateAsync(user);
            if (!ok.Succeeded) throw new InvalidOperationException("Bakiye güncellenemedi.");

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task CreditAsync(string userId, decimal amount)
        {

            if (amount <= 0) throw new ArgumentException("Amount must be >0");
            
            using var tx=await _context.Database.BeginTransactionAsync();
            var user=await _users.Users.FirstAsync(u=>u.Id == userId);

            user.Balance += amount;

            var ok= await _users.UpdateAsync(user);

            if (!ok.Succeeded) throw new InvalidOperationException("Bakiye güncellenemedi");

            await _context.SaveChangesAsync();
            await tx.CommitAsync();




                
        }
    }
}
