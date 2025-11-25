using GenericRepoWebApi.Dtos;
using GenericRepoWebApi.Entity;
using GenericRepoWebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        public ProductController(IRepository<Product> repository)
        {
            _repository = repository;   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products ?? Enumerable.Empty<Product>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            var productEntity = new Product(){
                Name = product.Name,
                Price = product.Price,
            };

            await _repository.AddAsync(productEntity);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto product)
        {
            var productEntity = await _repository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.Price = product.Price;
            productEntity.Name = product.Name;

            await _repository.UpdateAsync(productEntity);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var productEntity = await _repository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }


            await _repository.DeleteAsync(productEntity);
            return NoContent();
        }
    }
}
