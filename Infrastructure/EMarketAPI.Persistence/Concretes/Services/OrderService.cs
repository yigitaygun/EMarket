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
            
        public async Task CreateOrderAsync(CreateOrderDto createorderDtos) // Swagger'dan gelen sipariş verisini alır
        {


            // Yeni bir Order (sipariş) nesnesi oluşturuyoruz
            var order = new Order()    
            {
                UserId = createorderDtos.UserId, // Siparişi veren kullanıcının ID'si
                CreatedDate = DateTime.UtcNow,   // Sipariş tarihi (şu anki zaman)
                Items = new List<OrderItem>()   // Siparişin ürünlerini (OrderItem) tutacak liste
            };

            // Kullanıcının sipariş etmek istediği her ürün için dönüyoruz
            foreach (var item in createorderDtos.Items)
            {
                // Veritabanından ürün ID'sine göre ürünü çekiyoruz
                var product =await _productRepository.GetByIdAsync(item.ProductId);


                if (product == null)
                {
                    throw new Exception($"Ürün bulunamadı: {item.ProductId}");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new Exception($"Stok yetersiz: {product.Name}");
                }

                // Ürün bilgileriyle bir OrderItem (sipariş detayı) oluşturuyoruz
                var orderItem = new OrderItem()
                {
                    ProductId=product.Id, // Hangi ürün sipariş edildi
                    ProductName = product.Name,
                    Quantity =item.Quantity, // Kaç adet alındı
                    UnitPrice =product.Price // O anki birim fiyat  
                };

                // Siparişe bu ürünü ekliyoruz
                order.Items.Add(orderItem);

                // Ürünün stok bilgisini güncelliyoruz
                product.Stock-=item.Quantity;
                product.SoldCount += item.Quantity;

                // Ürünü güncelle (stok düşmüş halde veritabanına yazılacak)
                _productRepository.Update(product);
                }

                await _orderRepository.AddAsync(order); //Döngüden sonra tek seferde siparişi ekle

                await _unitOfWork.CommitAsync();   //Tek seferde tüm değişiklikleri veritabanına işle
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





    }
}
