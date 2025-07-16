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

namespace EMarketAPI.Persistence.Concretes.Services
{
    public class OrderService:IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;


        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task CreateOrderAsync(CreateOrderDto orderDto)
        {



            var order = _mapper.Map<Order>(orderDto);

            foreach(var item in order.Items)
            { 
                var product=await _productRepository.GetByIdAsync(item.ProductId);
                if(product != null)
                {
                    throw new Exception($"Ürün bulunamadı. ID: {item.ProductId}");
                }
                item.UnitPrice = product.Price;
                item.Product = product;
            }
            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

        }

        public Task<List<OrderDto>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
