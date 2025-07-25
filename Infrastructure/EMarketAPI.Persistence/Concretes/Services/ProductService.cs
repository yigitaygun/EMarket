using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Domain.Entities;
using EMarketAPI.Application.DTOs;
using EMarketAPI.Application.Abstractions.UnitOfWork;

namespace EMarketAPI.Persistence.Concretes.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }   

        public async Task AddProductAsync(CreateProductDto newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Ürün bulunamadı");

            product.IsDeleted = true;
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateProductAsync(UpdateProductDto updatedto)
        {
            var product = await _productRepository.GetByIdAsync(updatedto.Id);
            if (product != null)
                throw new Exception("Ürün bulunamadı.");


            product.Name= updatedto.Name;
            product.Price= updatedto.Price;
            product.Stock= updatedto.Stock;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
