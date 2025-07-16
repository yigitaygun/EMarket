using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Domain.Entities;

namespace EMarketAPI.Application.Abstractions.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsByCategoryAsync(string category); //belirli bir kategoriye ait ürünleri döner.
        Task<List<Product>> GetProductsInStockAsync();  //stokta olan ürünleri getir.
        Task<List<Product>> GetTopSellingProductsAsync(int count); //en çok satılan ürünleri getirir.
    }
}
