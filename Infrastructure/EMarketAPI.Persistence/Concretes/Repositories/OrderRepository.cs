    using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Domain.Entities;
using EMarketAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EMarketAPI.Persistence.Concretes.Repositories
{
    public class OrderRepository:GenericRepository<Order>,IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }   //buradaki contexti genericrepositorydeki constructora gönder.

        public async Task<List<Order>> GetAllOrdersWithItemsAsync()
        {
            return await _dbSet
                .Where(o => !o.IsDeleted)
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync();
        }


        // belirli bir kullanıcıya ait siparişleri getir
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync();
        }


        // Siparişi ve içindeki OrderItem'ları getir  //Belirli bir siparişin detayını (ürünleriyle birlikte) çekersin.
        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _dbSet
                .Include(o=>o.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);
        }

        // En son oluşturulan n adet siparişi getir
        public async Task<List<Order>> GetRecentOrdersAsync(int count)
        {

            return await _dbSet
                .Where(o => !o.IsDeleted)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedDate)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SoftDeleteAsync(int orderId)
        {
            var order=await _dbSet.FirstOrDefaultAsync(o=>o.Id==orderId);
            if (order != null)
                return;

            order.IsDeleted=true;
            _dbSet.Update(order);
        }
    }
}
