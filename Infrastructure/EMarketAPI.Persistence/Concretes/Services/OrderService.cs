using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.Abstractions.UnitOfWork;
using EMarketAPI.Application.DTOs;
using EMarketAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using EMarketAPI.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using EMarketAPI.Persistence.Context;

namespace EMarketAPI.Persistence.Concretes.Services
{
    public class OrderService:IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _ctx;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository,UserManager<AppUser> userManager,AppDbContext ctx)


        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
            _userManager = userManager;
            _ctx = ctx;
        }

        



        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersWithItemsAsync();

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.CreatedDate,
                Items = order.Items.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity, 
                    Price = oi.UnitPrice
                }).ToList(),
                TotalPrice = order.Items.Sum(x => x.UnitPrice * x.Quantity)

            }).ToList();
            return result;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId) //atıyorum sipariş no 5 i getir.
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);

            if (order == null)
                return null;
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.CreatedDate,
                Items = order.Items.Select(oi => new OrderItemDto {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    Price = oi.UnitPrice

                }).ToList(),
                TotalPrice = order.Items.Sum(x => x.UnitPrice * x.Quantity)
            };
        }



        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId)  //kullanıcı idsi 3 olanın siparişi getir.
        {
            var userorders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var donusum = userorders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.CreatedDate,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.UnitPrice
                }).ToList(),

                TotalPrice = order.Items.Sum(i => i.Quantity * i.UnitPrice)
            }).ToList();

            return donusum;
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.SoftDeleteAsync(orderId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<int> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
            if (dto.Items == null || dto.Items.Count == 0)
                throw new InvalidOperationException("En az bir ürün seçilmelidir.");

            using var tx=await _ctx.Database.BeginTransactionAsync();
            var order = new Order
            {
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                              ?? throw new KeyNotFoundException($"Ürün bulunamadı: {item.ProductId}");

                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException($"Stok yetersiz: {product.Name}");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                product.Stock -= item.Quantity;
                product.SoldCount += item.Quantity;
                _productRepository.Update(product);
            }

            var total=order.Items.Sum(i=>i.Quantity*i.UnitPrice);

            var user=await _ctx.Users.FirstAsync(u=>u.Id==userId);
            if (user.Balance < total)
                throw new InvalidOperationException("INSUFFICIENT_FUNDS");

            user.Balance-=total;

            _ctx.Users.Update(user);
            

            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();

            await tx.CommitAsync();
            return order.Id;




        }
    }
}
