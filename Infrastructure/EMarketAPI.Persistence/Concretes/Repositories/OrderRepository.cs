using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Domain.Entities;
using EMarketAPI.Persistence.Context;

namespace EMarketAPI.Persistence.Concretes.Repositories
{
    public class OrderRepository:GenericRepository<Order>,IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }   //buradaki contexti genericrepositorydeki constructora gönder.


        // belirli bir kullanıcıya ait siparişleri getir
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
           return await _dbSet
                .Where(o => o.UserId == userId)      
                .ToListAsync();
        }


        // Siparişi ve içindeki OrderItem'ları getir
        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // En son oluşturulan n adet siparişi getir
        public async Task<List<Order>> GetRecentOrdersAsync(int count)
        {

            return await _dbSet
                .OrderByDescending(o=>o.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
