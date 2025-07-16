using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.DTOs;

namespace EMarketAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task AddProductAsync(CreateProductDto newProduct);
        Task DeleteProductAsync(int id);
    }
}
