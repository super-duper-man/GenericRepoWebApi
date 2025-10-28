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
        public async Task<IResult> GetAllProducts()
        {
            return Results.Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);
        }

        [HttpPost]
        public async Task<IResult> CreateProduct([FromBody] ProductDto product)
        {
            var productEntity = new Product(){
                Name = product.Name,
                Price = product.Price,
            };

            await _repository.AddAsync(productEntity);

            return Results.Created();
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateProduct(int id, [FromBody] ProductDto product)
        {
            var productEntity = await _repository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return Results.NotFound();
            }

            productEntity.Price = product.Price;
            productEntity.Name = product.Name;

            await _repository.UpdateAsync(productEntity);

            return Results.NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IResult> RemoveProduct(int id)
        {
            var productEntity = await _repository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return Results.NotFound();
            }


            await _repository.DeleteAsync(productEntity);
            return Results.NoContent();
        }
    }
}
