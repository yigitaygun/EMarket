using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.DTOs;

namespace EMarketAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto orderDto);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId);
    }
}
