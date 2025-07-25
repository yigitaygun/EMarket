using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Domain.Entities;
using EMarketAPI.Persistence.Context;

namespace EMarketAPI.Persistence.Concretes.Repositories
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _dbSet.Where(p=>!p.IsDeleted).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsInStockAsync()
        {
            return await _dbSet
                .Where(p => p.Stock > 0)
                .ToListAsync();
        }

        public async Task<List<Product>> GetTopSellingProductsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(p=>p.SoldCount)
                .Take(count)
                .ToListAsync();
        }
    }
}
