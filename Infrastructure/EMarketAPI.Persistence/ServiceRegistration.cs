using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.Abstractions.UnitOfWork;
using EMarketAPI.Persistence.Concretes.Repositories;
using EMarketAPI.Persistence.Concretes.Services;
using EMarketAPI.Persistence.Concretes.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace EMarketAPI.Persistence
{
    public static class ServiceRegistration
    {   
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthService, AuthService>();
        }















    }
}
