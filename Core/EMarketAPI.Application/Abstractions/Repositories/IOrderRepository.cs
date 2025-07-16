using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Domain.Entities;

namespace EMarketAPI.Application.Abstractions.Repositories
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(string userId); // belirli bir kullanıcıya ait siparişleri getir
        Task<List<Order>> GetRecentOrdersAsync(int count); // en son verilen siparişleri getir

        Task<Order?> GetOrderWithItemsAsync(int orderId); // siparişin içinde hangi ürünler var
    }
}
