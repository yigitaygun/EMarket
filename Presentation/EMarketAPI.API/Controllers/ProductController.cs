using EMarketAPI.Application.Abstractions.Services;
using EMarketAPI.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMarketAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var products=await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var product=await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        // POST: api/products
        [HttpPost]

        public async Task<IActionResult> Add(CreateProductDto productdto)
        {
            await _productService.AddProductAsync(productdto);
            return Ok();
        }


        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id,UpdateProductDto updatedto)
        {
            updatedto.Id = id;
            await _productService.UpdateProductAsync(updatedto);
            return Ok();
        }

        // DELETE: api/products/5

        [HttpDelete("{id}")]

        public async Task <IActionResult> Delete (int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }




    }
}
