using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Domain.Entities;
using EMarketAPI.Application.DTOs;
using AutoMapper;

namespace EMarketAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<CreateOrderItemDto, OrderItem>();
        }
    }
}
